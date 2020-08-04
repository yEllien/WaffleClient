using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        public ChatRoomList(string cert)
        {
            InitializeComponent();
            LoadRooms(cert);
        }
    }
}