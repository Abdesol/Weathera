using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllLocations : ContentPage
    {
        public AllLocations()
        {
            InitializeComponent();
        }

        public async void Back_Clicked(object sender, EventArgs args)
        {
            await back_btn.ScaleTo(0.75, 100);
            await back_btn.ScaleTo(1, 100);

            App.Current.MainPage = new MainPage();
        }
    }
}