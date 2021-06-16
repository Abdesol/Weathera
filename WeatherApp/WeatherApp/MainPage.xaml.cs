using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Threading;
using System.IO;
using Android.Content.Res;
using Newtonsoft.Json.Linq;

namespace WeatherApp
{
    class DataBehind
    {
        public static bool isit = false;
        public static string title = "----------- ----------";
        public static string temp = "N/A";
        public static string wind = "N/A";
        public static string pressure = "N/A";
        public static string humidity = "N/A";
        public static string visibility = "N/A";
        public static bool isit_refresh = false;
        public static string weather_state_name = "N/A";
        public static string weather_state_abbr = null;
        public static string created = "---- 00, 00:00";
        public static string woeid = "-";

        public static bool isit_initial_open = true;

    }

    class New_db
    {
        public bool new_user { get; set; }
    }
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {   
            InitializeComponent();
            DataBehind.isit_refresh = false;
            if (DataBehind.isit == false)
            {
                Thread thr = new Thread(Init);
                thr.Start();
            }
            else
            {
                current_location.Text = DataBehind.title;
                temp_l.Text = DataBehind.temp;

                humidity_l.Text = DataBehind.humidity + "%";
                wind_l.Text = DataBehind.wind + " m/s";
                pressure_l.Text = DataBehind.pressure + " hpa";
                visibility_l.Text = DataBehind.visibility + "%";
                weather_desc.Text = DataBehind.weather_state_name;
                date_created.Text = DataBehind.created;
                cloud_type.Source = DataBehind.weather_state_abbr + ".png";


            }
            
        }

        private async void Init()
        {
            DataBehind.isit = true;

            if (!DataBehind.isit_refresh)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    App.Current.MainPage = new StartUp();
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    loading_label.IsVisible = true;
                });
            }

            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                string path = Path.Combine(FileSystem.AppDataDirectory, "db.json");
                if(DataBehind.isit_initial_open == true)
                {
                    try
                    {
                        string db_text = File.ReadAllText(path);
                        var d = JsonConvert.DeserializeObject<List<JObject>>(db_text);

                        var isfirst = d[0].ToObject<New_db>();
                        //await Task.Delay(3);

                        if (isfirst.new_user == false)
                        {
                            var second = d[1].ToObject<InitialJsonModel>();
                            DataBehind.woeid = second.Woeid.ToString();
                            Console.WriteLine(DataBehind.woeid);
                        }
                    }
                    catch { }
                }

                List<object> dat;
                if(DataBehind.woeid != "-")
                {
                    dat = await WeatherAPI.CurrentLocWeather(true, DataBehind.woeid);
                }
                else
                {
                    dat = await WeatherAPI.CurrentLocWeather(false, "");
                }

                await Task.Delay(2);

                InitialJsonModel data_ = (InitialJsonModel)dat[0];
                DataBehind.title = data_.Title;

                List<ConsolidatedWeather> data_all = data_.ConsolidatedWeather;

                ConsolidatedWeather data = data_all[data_all.Count - 1];

                int temp = (int)data.TheTemp;

                decimal wind = Decimal.Round((decimal)data.WindSpeed, 1);
                int pressure = (int)data.AirPressure;
                int humidity = (int)data.Humidity;
                int visibility = (int)data.Visibility;
                var weather_state_name = (string)data.WeatherStateName;

                var created = (DateTimeOffset)data.Created;



                DataBehind.temp = temp.ToString();
                DataBehind.wind = wind.ToString();
                DataBehind.pressure = pressure.ToString();
                DataBehind.humidity = humidity.ToString();
                DataBehind.visibility = visibility.ToString();
                DataBehind.weather_state_name = data.WeatherStateName;
                DataBehind.weather_state_abbr = data.WeatherStateAbbr;
                DataBehind.created = created.ToString("MMMM d, h:m");

                var new_db = new New_db();
                new_db.new_user = false;
                var l = new List<object>();
                for (int i = 0; i < data_all.Count; i++)
                {
                    if (i != data_all.Count - 1)
                    {
                        data_all.RemoveAt(i);
                    }
                }

                data_.ConsolidatedWeather = data_all;
                l.Add(new_db);
                l.Add(data_);

                var json = JsonConvert.SerializeObject(l);
                File.WriteAllText(path, json);
            }
            else
            {
                string path = Path.Combine(FileSystem.AppDataDirectory, "db.json");
                try
                {
                    string db_text = File.ReadAllText(path);
                    var d = JsonConvert.DeserializeObject<List<JObject>>(db_text);

                    var isfirst = d[0].ToObject<New_db>();
                    await Task.Delay(5);

                    if (isfirst.new_user == false)
                    {
                        var initial = d[1].ToObject<InitialJsonModel>();

                        DataBehind.title = initial.Title;

                        List<ConsolidatedWeather> data_all = initial.ConsolidatedWeather;

                        ConsolidatedWeather data = data_all[0];

                        int temp = (int)data.TheTemp;

                        decimal wind = Decimal.Round((decimal)data.WindSpeed, 1);
                        int pressure = (int)data.AirPressure;
                        int humidity = (int)data.Humidity;
                        int visibility = (int)data.Visibility;
                        var weather_state_name = (string)data.WeatherStateName;

                        var created = (DateTimeOffset)data.Created;


                        DataBehind.temp = temp.ToString();
                        DataBehind.wind = wind.ToString();
                        DataBehind.pressure = pressure.ToString();
                        DataBehind.humidity = humidity.ToString();
                        DataBehind.visibility = visibility.ToString();
                        DataBehind.weather_state_name = data.WeatherStateName;
                        DataBehind.weather_state_abbr = data.WeatherStateAbbr;
                        DataBehind.created = created.ToString("MMMM d, h:m");

                    }
                }
                catch
                {
                    var l = new List<object>();
                    l.Add(new New_db());
                    l.Add(new InitialJsonModel());

                    var json = JsonConvert.SerializeObject(l);
                    File.WriteAllText(path, json);
                }

                Acr.UserDialogs.UserDialogs.Instance.Toast("No Internet Connection!", new TimeSpan(5));
            }

            await Task.Delay(1);
            if (!DataBehind.isit_refresh)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    App.Current.MainPage = new MainPage();
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    loading_label.IsVisible = false;
                });
            }

            DataBehind.isit_initial_open = false;

        }

        public void RefreshBtn_Clicked(object sender, EventArgs args)
        {
            async void animation()
            {
                await refresh_btn.ScaleTo(0.75, 80);
                await refresh_btn.ScaleTo(1, 80);
            }
            animation();
            DataBehind.isit_refresh = true;
            var current = Connectivity.NetworkAccess;
            
            if (current == NetworkAccess.Internet)
            {
                Init();
            }
            else
            {
                Acr.UserDialogs.UserDialogs.Instance.Toast("No Internet Connection!", new TimeSpan(3));
            }
            current_location.Text = DataBehind.title;
            temp_l.Text = DataBehind.temp;

            humidity_l.Text = DataBehind.humidity + "%";
            wind_l.Text = DataBehind.wind + " m/s";
            pressure_l.Text = DataBehind.pressure + " hpa";
            weather_desc.Text = DataBehind.weather_state_name;
            visibility_l.Text = DataBehind.visibility + "%";
            date_created.Text = DataBehind.created;
            cloud_type.Source = DataBehind.weather_state_abbr + ".png";

        }

        public async void ChangeLoc_Clicked(object sender, EventArgs args)
        {
            await location_btn.ScaleTo(0.75, 60);
            await location_btn.ScaleTo(1, 60);

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                await Navigation.PushModalAsync(new AllLocations(), true);
            }
            else
            {
                Acr.UserDialogs.UserDialogs.Instance.Toast("No Internet Connection!", new TimeSpan(3));
            }
            
        }
    }
}
