using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace chatty
{
    public partial class App : Application
    {

        

        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();
            NavigationPage navigation = new NavigationPage();

            string cert = ((LoginPage)MainPage).GetCertification();
           
        
            /*
            string cert = ((LoginPage)MainPage).GetCertification();

            if (cert != "Empty")
                navigation.PushAsync(new ChatRoomList(cert));

            //failed
             */
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
