using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace chatty
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        //lists are for demo
        private List<string> users = new List<string>() 
        {
            "Tony","Ellie","Kanivalaki","Carnivore"
        };
        private List<string> displaynames = new List<string>()
        {
            "Tonyton ton", "Ello jello", "Kan", "Nick the greek"
        };

        public int LookUp (string target)
        {
            for (int i=0; i<users.Count; i++)
                if (users[i] == target) return i;
            return -1;
        }

        public void SearchUser(object sender, EventArgs args)
        {
            //search fsjkdjdk
            //demo
            //request results
            Results.Children.Clear();
            Application.Current.MainPage.BackgroundColor = Color.White;
            int index = LookUp(searchbar.Text);
            if(index==-1)
            {
                Label error = new Label()
                {
                    Text = "No results for '" + searchbar.Text + "'",
                    TextColor = Color.DimGray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    FontSize = 18
                };

                Results.Children.Add(error);
            }
            else
            {
                StackLayout result = new StackLayout
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Center
                };

                Label Name = new Label()
                {
                    Text = displaynames[index],
                    TextColor = Color.White,
                    FontFamily = "Solway-Light.ttf#Solway Light",
                    FontSize = 30,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Padding = new Thickness(0, 0, 0, 5)
                };

                Label Username = new Label()
                {
                    Text = users[index],
                    TextColor = Color.DimGray,
                    FontSize = 19,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Padding = new Thickness(0, 5, 0, 5)
                };

                Button Send = new Button()
                {
                    Text = "Send Request",
                    TextColor = Color.FromHex("#343434"),
                    BackgroundColor = Color.FromHex("#ffb940"),
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Margin=new Thickness(20)
                };

                result.Children.Add(Name);
                result.Children.Add(Username);
                result.Children.Add(Send);

                Results.Children.Add(result);
            }

        }

        public SearchPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#222222");

            //list stuff is for demo

            
        }
    }
}