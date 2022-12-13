using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FunnyForecasts
{
    public class ForecastService
    {
        HttpClient _httpClient { get; set; }

        string API_URL { get; } = "api.openweathermap.org/data/2.5/forecast/daily?q=London&units=metric&cnt=7&appid={API key}";

        List<string> WEATHER_OPTIONS { get; } = new List<string>() { "Thunderstorm", "Drizzle", ""  }
        public ForecastService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetRandomApiCall()
        {
            var response = await _httpClient.GetAsync("https://official-joke-api.appspot.com/random_joke");
            return await response.Content.ReadAsStringAsync();
        }
        
        public async Task<bool> GetForecast()
        {
            // TODO: What forecasts can I even get?
            // Thunderstorm, Drizzle, Rain, Snow, Mist, Fog, Clear, Clouds
            var response = await _httpClient.GetAsync(API_URL);
            var data = await response.Content.ReadAsStringAsync();

            var forecastData = JsonConvert.DeserializeObject<ForecastData>(data);
            return true; // Just to stop compile errors.

        }

        public class ForecastData
        {
            public List<WeatherData> List { get; set; }
        }

        public class WeatherData
        {
            public long Dt { get; set; }
            public JObject Main { get; set; }
            public List<JObject> Weather { get; set; }
        }
    }
}
