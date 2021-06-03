using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {   
            InitializeComponent();

            locations.Title = "Locations"; locations.TextColor = Color.White;

            List<string> all_locations = new List<string>() 
            {"Addis Ababa, ET",
            "Seattle, WA",
            "LosAngles, CA",
            "Diredwa, ET",
            "Nairobi, KE",
            "Mekele, ET",
            "Mumbai, IN",
            "Tokyo, JP"
            };

            foreach(string loc in all_locations)
            {
                locations.Items.Add(loc);
            }

            locations.SelectedIndexChanged += (sender, args) =>
            {
                if(locations.SelectedIndex == -1)
                {
                    current_location.Text = all_locations[0];
                }
                else
                {
                    current_location.Text = locations.Items[locations.SelectedIndex];
                }
            };
        }
        
        public async void RefreshBtn_Clicked(object sender, EventArgs args)
        {
            await refresh_btn.ScaleTo(0.75, 60);
            await refresh_btn.ScaleTo(1, 60);
            for (int i=1; i<=6; i++)
            {
                if (refresh_btn.Rotation >= 360f) refresh_btn.Rotation = 0;
                await refresh_btn.RotateTo(i * (360 / 6), 1000, Easing.Linear);
            }
        }

        public async void ChangeLoc_Clicked(object sender, EventArgs args)
        {
            await location_btn.ScaleTo(0.75, 100);
            await location_btn.ScaleTo(1, 100);

            App.Current.MainPage = new AllLocations();
        }
    }
}
