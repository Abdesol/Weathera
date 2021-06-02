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
            //top_frame.BackgroundColor = Color.FromHex(ViewModel.half_screen_color);
            //top_frame.HeightRequest = ViewModel.half_screen_size;
        }
        
        public async void MenuBtn_Clicked(object sender, EventArgs args)
        {
            await menu_btn.ScaleTo(0.75, 100);
            await menu_btn.ScaleTo(1, 100);
        }

    }
}
