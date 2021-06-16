using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp
{
    class InitialJsonModel
    {
        [JsonProperty("consolidated_weather")]
        public List<ConsolidatedWeather> ConsolidatedWeather { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("woeid")]
        public long Woeid { get; set; }
    }

    public partial class ConsolidatedWeather
    {

        [JsonProperty("weather_state_name")]
        public string WeatherStateName { get; set; }

        [JsonProperty("weather_state_abbr")]
        public string WeatherStateAbbr { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("the_temp")]
        public double TheTemp { get; set; }

        [JsonProperty("wind_speed")]
        public double WindSpeed { get; set; }

        [JsonProperty("air_pressure")]
        public double AirPressure { get; set; }

        [JsonProperty("humidity")]
        public long Humidity { get; set; }

        [JsonProperty("visibility")]
        public double Visibility { get; set; }
    }


    public class LeftJsonModel
    {
        [JsonProperty("weather_state_name")]
        public string WeatherStateName { get; set; }

        [JsonProperty("weather_state_abbr")]
        public string WeatherStateAbbr { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("the_temp")]
        public double? TheTemp { get; set; }

        [JsonProperty("wind_speed")]
        public double? WindSpeed { get; set; }

        [JsonProperty("air_pressure")]
        public double? AirPressure { get; set; }

        [JsonProperty("humidity")]
        public long? Humidity { get; set; }

        [JsonProperty("visibility")]
        public double? Visibility { get; set; }

    }

    public partial class SearchModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("woeid")]
        public long Woeid { get; set; }

    }
}
