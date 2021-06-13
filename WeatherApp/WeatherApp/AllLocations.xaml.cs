using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public string Search_result_text { get; set; }
        public AllLocations()
        {
            InitializeComponent();
            for (int i = 0; i <= 12; i++)
            {
                search_layout.Children.Add(new Label { Text = "Addis Ababa, ET", FontSize = 20, TextColor = Color.White });
            }
        }

        public async void Back_Clicked(object sender, EventArgs args)
        {
            await back_btn.ScaleTo(0.75, 60);
            await back_btn.ScaleTo(1, 60);
            await Navigation.PopModalAsync(true);
        }

        public async void Search_Clicked(object sender, EventArgs args)
        {
            await search_btn.ScaleTo(0.75, 60);
            await search_btn.ScaleTo(1, 60);

            string search_text = search_entry.Text;
            

            
        }
    }
}