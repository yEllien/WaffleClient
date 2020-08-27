using System;
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
            if (SB.Content.Height != SB.Height) QuickScroll.IsVisible = true;
            else QuickScroll.IsVisible = false;

            // i need the line below to get the proper string. for now ill make some fake messages so i can debug
            //var responseString = await client.GetStringAsync(new Uri("http://192.168.2.3/chatservice/chat/"+most_recent_id)); 

           var responseString = "0,Ellie,Hey there,00:01:50,1,Tony,Hi,00:02:32,2,Tony,Lorem ipsum,00:01:54,4,Ellie,dolor sit amet,00:03:9,5,Tony,consectetur adipiscing elit,00:07:06,6,Ellie,sed do eiusmod tempor incididunt ut labore et dolore magna aliqua,00:08:57,7,Tony, Ut enim ad minim veniam,16:20:03,8,Ellie,quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident,16:25:04,9,Tony,sunt in culpa qui officia deserunt mollit anim id est laborum.,20:12:45,10,Ellie,Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium totam rem aperiam eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt.,22:45:55";

           
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
                    
                    source = new Label()
                    {
                        FontSize = 13,
                        Text = tmp.name,
                        TextColor = Color.FromHex("#656565")
                    };

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
            SBHeight.Height = Application.Current.MainPage.Height - 150;
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
