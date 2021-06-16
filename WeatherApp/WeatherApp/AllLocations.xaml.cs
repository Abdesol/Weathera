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
        public List<SearchModel> search_res;
        public AllLocations()
        {
            InitializeComponent();
            /*
            for (int i = 0; i <= 12; i++)
            {
                search_layout.Children.Add(new Label { Text = "Addis Ababa, ET", FontSize = 20, TextColor = Color.White });
            }*/
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
            search_layout.Children.Clear();
            search_layout.Children.Add(new Label
            {
                Text = "loading...",
                FontSize = 20,
                TextColor = Color.White,
                Opacity = 10,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            });


            try
            {
                var result = await WeatherAPI.SearchLoc(search_text);
                search_res = result;
                if(result.Count == 0)
                {
                    search_layout.Children.Clear();
                    search_layout.Children.Add(new Label
                    {
                        Text = "No result :(",
                        FontSize = 20,
                        TextColor = Color.White,
                        Opacity = 10,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center
                    });
                }
                else
                {
                    if (result[0].Title == "Error")
                    {
                        await Navigation.PopModalAsync(true);
                        Acr.UserDialogs.UserDialogs.Instance.Toast("No Internet Connection!", new TimeSpan(5));
                    }
                    else
                    {
                        search_layout.Children.Clear();
                        foreach (SearchModel ser in result)
                        {
                            var btn = new Button
                            {
                                Text = ser.Title,
                                FontSize = 20,
                                TextColor = Color.White,
                                BackgroundColor = Color.Transparent
                            };
                            btn.Clicked += Result_clicked;

                            search_layout.Children.Add(btn);

                        }
                    }

                }

            }
            catch
            {
                await Navigation.PopModalAsync(true);
                Acr.UserDialogs.UserDialogs.Instance.Toast("No Internet Connection!", new TimeSpan(5));
            }
        }

        public async void Result_clicked(object sender, EventArgs args)
        {
            var clicked_btn = (Button)sender;
            await clicked_btn.ScaleTo(0.75, 60);
            await clicked_btn.ScaleTo(1, 60);

            int i = 0;
            foreach(var btn in search_layout.Children)
            {
                if(btn == clicked_btn) { break; }
                i++;
            }

            var clicked_btn_data = search_res[i];
            DataBehind.isit = false;
            DataBehind.woeid = clicked_btn_data.Woeid.ToString();
            await Navigation.PushModalAsync(new MainPage());
        }
    }
}