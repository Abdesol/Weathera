using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using RestSharp;

namespace WeatherApp
{
    class WoiedModel
    {
        public int woeid { get; set; } 
    }
    public static class WeatherAPI
    {

        public static async Task<List<object>> CurrentLocWeather(bool iswoe, string woe)
        {
            var output = new List<object>();
            var er = new Dictionary<string, object>();
            er.Add("Error", "True");
            string woeid_url = "https://www.metaweather.com/";
            async Task<string> GetWoeid()
            {
                string city = "";
                string ip_url = "https://ipinfo.io";
                
                int woeid_ = 0;

                var client = new HttpClient();
                client.BaseAddress = new Uri(ip_url);
                client.DefaultRequestHeaders.Accept.Clear();
                bool isip_done = false;
                for (int i = 0; i <3; i++)
                {
                    HttpResponseMessage response = await client.GetAsync($"json");
                    if (response.IsSuccessStatusCode)
                    {
                        string info = await response.Content.ReadAsStringAsync();
                        var info_parsed = JsonConvert.DeserializeObject<Dictionary<string, string>>(info);
                        city = info_parsed["city"];
                        isip_done = true;
                        break;
                    }
                    await Task.Delay(1);
                }
                if (isip_done == false)
                {
                    return "Error";
                }


                var woe_client = new HttpClient();
                woe_client.BaseAddress = new Uri(woeid_url);
                woe_client.DefaultRequestHeaders.Accept.Clear();
                bool iswoe_done = false;
                for (int i = 0; i < 3; i++)
                {
                    HttpResponseMessage response = await woe_client.GetAsync($"api/location/search/?query={city}");
                    if (response.IsSuccessStatusCode)
                    {
                        string woeid_request = await response.Content.ReadAsStringAsync();
                        var woeid_parsed = JsonConvert.DeserializeObject<List<WoiedModel>>(woeid_request);
                        WoiedModel w = woeid_parsed[0];
                        woeid_ = w.woeid;
                        iswoe_done = true;
                        break;
                    }
                    await Task.Delay(1);
                }
                if (iswoe_done == false)
                {
                    return "Error";
                }
                
                return woeid_.ToString();

            }
            string woeid = "";
            if (iswoe == false)
            {
                woeid = await GetWoeid();

            }
            else 
            {
                woeid = woe;
            }

            if (woeid != "Error")
            {
                var days_lst = new List<string>();
                for(int i = 1; i>=-6; i--)
                {
                    string date = DateTime.Today.AddDays(i).Date.ToString("yyyy/MM/dd");
                    days_lst.Add(date);
                }
                

                var client = new HttpClient();
                client.BaseAddress = new Uri(woeid_url);
                client.DefaultRequestHeaders.Accept.Clear();

                bool isinitial_done = false;
                for (int ii = 0; ii < 3; ii++)
                {
                    HttpResponseMessage initial_response = await client.GetAsync($"api/location/{woeid}");
                    if (initial_response.IsSuccessStatusCode)
                    {
                        string initial_data_request = await initial_response.Content.ReadAsStringAsync();
                        var initial_data_parsed = JsonConvert.DeserializeObject<InitialJsonModel>(initial_data_request);

                        output.Add(initial_data_parsed);

                        isinitial_done = true;
                        break;
                    }
                    await Task.Delay(1);
                }
                if (isinitial_done == false)
                {
                    return new List<object>() { er };
                }
            }
            else
            {
                var aa = new Dictionary<string, object>();
                aa.Add("Error", "false");

                return new List<object>() { aa };
            }

            return output;

        }

        public static async Task<List<SearchModel>> SearchLoc(string search_text)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://www.metaweather.com/");
                client.DefaultRequestHeaders.Accept.Clear();

                HttpResponseMessage response = await client.GetAsync($"api/location/search/?query={search_text}");
                if (response.IsSuccessStatusCode)
                {
                    string info = await response.Content.ReadAsStringAsync();
                    var info_parsed = JsonConvert.DeserializeObject<List<SearchModel>>(info);

                    return info_parsed;
                }
                else
                {
                    var e = new SearchModel();
                    e.Title = "Error";
                    var ee = new List<SearchModel>();
                    ee.Add(e);

                    return ee;
                }
            }
        }
    }
}
