﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using chatty;
using System.Diagnostics.CodeAnalysis;
using Xamarin.Forms.Markup;

namespace chatty
{
    
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ChatRoom : ContentPage
    {
        public const int comma = int.MaxValue - 2;
        private static HttpClient client = new HttpClient();
        public string most_recent_id;
        List<Message> messages = new List<Message>();

        public static string BreakText (string text)
        {
            string tmp = string.Empty;
            int i = 0, j=0, lastspace=0, afternewline=0;
            
            while (i<text.Length)
            {
                if (text[i] == ' ') lastspace = i;

                if (i/20>j)
                {
                    if (lastspace / 20 == j)
                        tmp = tmp + text.Substring(afternewline, lastspace - afternewline) + "\n";
                    else tmp = tmp + text.Substring(afternewline, i-afternewline) + "\n";
                    afternewline = i = lastspace + 1;
                    j++;
                }
                i++;
            }

            return tmp;
        }

        public async void Refresh() 
        {
            // i need the line below to get the proper string. for now ill make some fake messages so i can debug
            //var responseString = await client.GetStringAsync(new Uri("http://192.168.2.3/chatservice/chat/"+most_recent_id)); 

            var responseString = "0,Ellie,Hey there,00:01:50,1,Tony,Hi,00:02:32,2,Tony,Sup,00:01:54,4,Ellie,All good ig,00:03:9,5,Tony,im gay,00:07:06,6,Ellie,i know darling,00:08:57,7,Tony,i hate u,16:20:03,8,Ellie,rly?,16:25:04,9,Tony,HIHIHIHIHIIIHH ASD this is some test text and im tryign t osee if the mesage wraps or not and if not ig ill ave t ofix it lets seeeeeeeeeeee letssee and where tho a a a a a a a a a a a a a aa a a a aaa,20:12:45,10,Ellie,um ok then ill tet it myself too lets seeee how long it gets hm i hope this waps cause i rly hate xamarin i hope i wont have to bother toom uch anymorei  just wanna build shit !!!,22:45:55";

            

            StackLayout line;

            Label time = new Label();

            Label message = new Label();

            Label source = new Label();

            Label seperator;

            if (responseString == "failed")
            {
                message = new Label();
                message.Text = "Failed to load messages";
                message.TextColor = Color.Red;
                message.BackgroundColor = Color.PaleVioletRed;
                message.HorizontalTextAlignment = TextAlignment.Center;
            }
            else
            {
                List<string> data = new List<string>();
                data = responseString.Split(',').ToList();

                Message tmp = new Message();

                int i = 0;
                while (i <= data.Count() - 4)
                {
                    tmp.id = data[i++];
                    tmp.name = data[i++];
                    tmp.content = data[i++];
                    tmp.time = data[i++];
                    messages.Add(tmp);

                    string txt = tmp.content.Replace(comma.ToString(), ",");

                    message = new Label()
                    {
                        FontSize = 16,
                        Text = txt,
                        FontFamily = "Solway-Light.ttf#Solway Light",
                        HorizontalTextAlignment = TextAlignment.Start
                    };
                    /*
                    message.FontSize = 16;
                    message.Text = txt;
                    message.FontFamily = "Solway-Light.ttf#Solway Light";
                    message.HorizontalTextAlignment = TextAlignment.Start;
                    */
                    /*
                    source = new Label();
                    source.FontSize = 13;
                    source.Text = tmp.name;
                    source.TextColor = Color.FromHex("#656565");
                    */
                    source = new Label()
                    {
                        FontSize = 13,
                        Text = tmp.name,
                        TextColor = Color.FromHex("#656565")
                    };

                    /*
                    time = new Label();
                    time.TextColor = Color.FromHex("#656565"); 
                    time.FontSize = 11;
                    time.FontFamily = "Solway-Light.ttf#Solway Light";
                    time.Text = tmp.time;
                    */
                    time = new Label()
                    {
                        TextColor = Color.FromHex("#656565"),
                        FontSize = 11,
                        FontFamily = "Solway-Light.ttf#Solway Light",
                        Text = tmp.time
                    };

                    line = new StackLayout() { Orientation = StackOrientation.Horizontal };
                    
                    if (tmp.name == "Ellie")
                    {
                        message.TextColor = Color.FromHex("#ffb940");
                        message.HorizontalOptions = LayoutOptions.End;
                        message.Padding = new Thickness(30, 0, 10, 1);

                        time.HorizontalTextAlignment = TextAlignment.End;

                        line.Padding = new Thickness(30, 1, 10, 0);

                        line.HorizontalOptions = LayoutOptions.EndAndExpand;
                        line.Children.Add(time);  
                    }
                    else
                    {
                        message.TextColor = Color.LightGray;
                        message.HorizontalOptions = LayoutOptions.Start;
                        message.Padding = new Thickness(10, 0, 30, 1);

                        source.HorizontalTextAlignment = TextAlignment.Start;

                        time.HorizontalTextAlignment = TextAlignment.Start;
                        time.VerticalTextAlignment = TextAlignment.End;

                        seperator = new Label { Text = "•", FontSize = 13, TextColor = Color.FromHex("#656565") };

                        line.Padding = new Thickness(10, 1, 30, 0);
                        line.HorizontalOptions = LayoutOptions.StartAndExpand;
                        
                        line.Children.Add(source);
                        line.Children.Add(seperator);
                        line.Children.Add(time);
                    }

                    SLV.Children.Add(line);                                               
                    SLV.Children.Add(message);
                }
                if (data.Count > 1)
                {
                    most_recent_id = tmp.id;
                    //await SB.ScrollToAsync(0, SLV.Y, false); ;                            idk
                }
            }
        }

        public async void InitialRefresh()
        {
            SLV.Children.Clear();
            Refresh();
        }

        public async void HandleButtonClick(object Sender, EventArgs args)
        {
            client.PostAsync(new Uri("http://192.168.2.3/chatservice/chat/Post/" + "Ellie" + "/" + Input.Text), null);
            Input.Text = string.Empty;
            Post.Source = "logoicon.png";
            Refresh();
        }

        public void PressedIcon (object Sender, EventArgs args)
        {
            Post.Source = "pressicon.png";
        }
           
        public ChatRoom()
        {
            InitializeComponent();
            InitialRefresh();
            SB.HeightRequest = Application.Current.MainPage.Height-10;

            /* ONLY A COMMENT TO DEBUG THE UI!
            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
            {
                Refresh();
                return true;
            });
            */
        }
    }
}
