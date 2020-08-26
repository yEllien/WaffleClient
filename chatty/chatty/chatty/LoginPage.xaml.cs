using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace chatty
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private static HttpClient client = new HttpClient();
        private string certification;

        public string GetCertification ()
        {
            return certification;
        }

        public void Login (object sender, EventArgs args)
        {
            ChatRoom chatlist = new ChatRoom();
            Application.Current.MainPage = chatlist;
            /*
             
            var cert = client.GetStringAsync(new Uri("http://192.168.2.3/chatservice/chat/Post/Login/" + Username.Text+"/"+Password.Text));
            certification = cert.ToString();
            Username.Text = "";
            Password.Text = "";


            if(cert.ToString() == "Empty")
            {
                Label error = new Label();
                error.Text = "Unable to login";
            }

             */
        }
        public LoginPage()
        {
            InitializeComponent();
            certification = string.Empty;
        }
    }
}