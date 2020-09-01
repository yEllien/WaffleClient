using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace chatty
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatRoomList : ContentPage
    {


        private static HttpClient client = new HttpClient();

        public void LoadChatRoom(object sender, EventArgs args)
        {
            string room = (sender as Button).Text;
            client.GetStringAsync(new Uri("http://192.168.2.3/chatservice/chat/GetRooms/"));
        }
        public void LoadRooms (string cert)
        {
            var data = client.GetStringAsync(new Uri("http://192.168.2.3/chatservice/chat/GetRooms/" + cert));
            List<string> Rooms = new List<string>();
            Rooms = data.ToString().Split(',').ToList();

            Button button;

            for (int i = 0; i<Rooms.Count; i++)
            {
                button = new Button();
                button.Text = Rooms[i];
                button.TextColor = Color.CadetBlue;
                button.BorderColor = Color.LightGray;
                button.Padding = 15;
                button.HorizontalOptions = LayoutOptions.StartAndExpand;
                button.VerticalOptions = LayoutOptions.Center;
                button.Clicked += LoadChatRoom;

                SL.Children.Add(button);
            }

        }

        public void LoadChatRoom ()
        {
            ChatRoom chatRoom = new ChatRoom();
            Navigation.PushAsync(chatRoom);
        }

        public void Test(string username, string lasttime, string lastmessage, string status)
        {
            StackLayout chat = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Padding = 20
            };
            
            TapGestureRecognizer TapRoom = new TapGestureRecognizer()
            {
                Command = new Command(() => LoadChatRoom())
            };
            
            Image StatusIcon = new Image()
            {
                Source = status+".png",
                Scale = 0.15,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                Margin = -35
            };

            Label Name = new Label()
            {
                Text = username,
                //TextColor = Color.FromHex("#ffb940"),
                TextColor = Color.White,
                FontSize = 22,
                FontFamily = "Solway-Light.ttf#Solway Light",
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center
            };


            /* I cant retrieve this info yet, to be added when such request exists
            StackLayout preview = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center
            };
            
            Label Time = new Label()
            {
                Text = lasttime,
                TextColor = Color.LightGray,
                FontSize = 8,
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.End
            };

            Label MessagePreview = new Label()
            {
                Text = lastmessage,
                TextColor = Color.LightGray,
                FontSize = 10,
                FontFamily = "Solway-Light.ttf#Solway Light",
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Start
            };
            */

            BoxView Seperator = new BoxView()
            {
                HeightRequest = 1,
                WidthRequest = Application.Current.MainPage.Width - 30,
                HorizontalOptions = LayoutOptions.Center,
                Color = Color.DimGray
            };

            //preview.Children.Add(Time);
            //preview.Children.Add(MessagePreview);

            chat.Children.Add(StatusIcon);
            chat.Children.Add(Name);
            //chat.Children.Add(preview);
            
            chat.GestureRecognizers.Add(TapRoom);

            ChatsListLayout.Children.Add(chat);
            ChatsListLayout.Children.Add(Seperator);
            
        }

        public ChatRoomList(string cert)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            ChatsListScrollView.HeightRequest = Application.Current.MainPage.Height;
            Test("Tony","00:00:20","Hi", "active");
            Test("Kanivalaki", "16:23:54", "I'm an alien !", "inactive");
            Test("I died yesterday", "16:23:54", "I'm an alien !", "longinactive");
            Test("I died yesterday", "16:23:54", "I'm an alien !", "longinactive");
            Test("I died yesterday", "16:23:54", "I'm an alien !", "longinactive");
            Test("I died yesterday", "16:23:54", "I'm an alien !", "longinactive");
            Test("I died yesterday", "16:23:54", "I'm an alien !", "longinactive");
            Test("I died yesterday", "16:23:54", "I'm an alien !", "longinactive");
            Test("I died yesterday", "16:23:54", "I'm an alien !", "longinactive");
            //LoadRooms(cert);

        }
    }
}