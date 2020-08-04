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
            var responseString = await client.GetStringAsync(new Uri("http://192.168.2.3/chatservice/chat/"+most_recent_id));

            Grid grid = new Grid() ;

            Label time = new Label();

            Label message = new Label();

            Label source = new Label();

            if(responseString == "failed")
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
                while (i < data.Count() - 4)
                {
                    tmp.id = data[i++];
                    tmp.name = data[i++];
                    tmp.content = data[i++];
                    tmp.time = data[i++];
                    messages.Add(tmp);

                    message = new Label();
                    message.TextColor = Color.White; 
                    message.FontSize = 16;
                    string txt = tmp.content.Replace(comma.ToString(), ",");
                    message.Text = txt;
                    message.Padding = 10;

                    source = new Label();
                    source.FontSize = 13;
                    source.Text  = tmp.name;

                    time = new Label();
                    time.TextColor = Color.SlateGray; 
                    time.FontSize = 11;
                    time.Text = tmp.time;

                    //grid = new Grid();

                    if (tmp.name == "Ellie")
                    {
                        message.BackgroundColor = Color.CadetBlue;
                        message.HorizontalTextAlignment = TextAlignment.End;
                        time.HorizontalTextAlignment = TextAlignment.End;
                        
                    //    grid.HorizontalOptions = LayoutOptions.StartAndExpand;
                    //    grid.Children.Add(time, 0, 1,0,1);
                    //    grid.Children.Add(message, 2, 2);
                    }
                    else
                    {
                        message.BackgroundColor = Color.FromRgb(0.62,0.69,0.68);
                        message.HorizontalTextAlignment = TextAlignment.Start;
                        source.HorizontalTextAlignment = TextAlignment.Start;
                        time.HorizontalTextAlignment = TextAlignment.Start;
                        time.VerticalTextAlignment = TextAlignment.End;

                    //    grid.Children.Add(source,0,0);
                    //    grid.Children.Add(time, 1,8,0,1);
                     //   grid.HorizontalOptions = LayoutOptions.StartAndExpand;
                    }


                //    SLV.Children.Add(grid);
                    SLV.Children.Add(message);
                }
                if (data.Count > 1)
                {
                    most_recent_id = tmp.id;
                    //await SB.ScrollToAsync(0, SLV.Y, false); ;
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
            Refresh();
        }


           
        public ChatRoom()
        {
            InitializeComponent();
            InitialRefresh();
            
            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
            {
                Refresh();
                return true;
            });
        }
    }
}
