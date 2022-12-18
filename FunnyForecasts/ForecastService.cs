using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FunnyForecasts
{
    public class ForecastService
    {
        HttpClient _httpClient { get; set; }

        string WEATHER_API_URL { get; } = "https://api.openweathermap.org/data/2.5/forecast?q=London&appid=86241482320999b78584c5d6c9374679&cnt=5";
        string JOKE_API_URL { get; } = "https://v2.jokeapi.dev/joke/Dark?blacklistFlags=racist&type=twopart&amount=1";

        List<string> RAIN_CONDITIONS = new List<string>() { "Thunderstorm", "Drizzle", "Rain" };
        List<string> FOG_CONDITIONS = new List<string>() { "Mist", "Fog" };
        List<string> GOOD_CONDITIONS = new List<string>() { "Clear", "Clouds" };
        string SNOW_CONDITIONS = "Snow";


        public ForecastService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> BuildForecastNotification()
        {
            var weatherForecast = await GetWeatherForecast();
            var joke = await GetRandomJoke();

            return weatherForecast + "\nHere's a joke: " + joke;
        }
        
        public async Task<string> GetWeatherForecast()
        {
            var response = await _httpClient.GetAsync(WEATHER_API_URL);
            var data = await response.Content.ReadAsStringAsync();

            var forecastData = JsonConvert.DeserializeObject<ForecastData>(data);
            var dayWeatherConditions = new List<WeatherData>();
            double temp = 0;
            double feels_like = 0;

            foreach (var forecast in forecastData.list)
            {
                var hourlyWeatherConditions = forecast.weather.Select(weatherCondition => new WeatherData()
                {
                    main = weatherCondition.Value<string>("main"),
                    description = weatherCondition.Value<string>("description")
                }); // TODO: Extract.

                dayWeatherConditions.AddRange(hourlyWeatherConditions);

                temp += forecast.main.Value<double>("temp");
                feels_like += forecast.main.Value<double>("feels_like");
            }

            temp = temp / forecastData.list.Count;
            feels_like = feels_like/ forecastData.list.Count;

            var weatherMessage = BuildWeatherNotificationMessage(dayWeatherConditions, (int)temp, (int)feels_like);

            return weatherMessage;
        }

        private string BuildWeatherNotificationMessage(List<WeatherData> dayWeatherConditions, int temp, int feels_like)
        {
            var badWeatherConditions = dayWeatherConditions.Where(condition => !GOOD_CONDITIONS.Contains(condition.main));
            var weatherMessage = "";

            if (badWeatherConditions.Count() > 0)
            {
                weatherMessage = BuildBadWeatherMessage(badWeatherConditions);
            }
            else
            {
                var morningCondition = dayWeatherConditions.FirstOrDefault();
                weatherMessage = $"It's looking good - just {morningCondition.description} when you leave. Stay sexy.";
            }

            var tempMessage = $"The average temp today is {temp} degrees but it should feel like {feels_like}. Today is going to be a good day. I love you.";

            return $"{weatherMessage} {tempMessage}"; 
        }

        private string BuildBadWeatherMessage(IEnumerable<WeatherData> badWeatherConditions)
        {
            // TODO: Find the most frequent one and then use the switch to return a different message for each.
            var dayWeather = badWeatherConditions.GroupBy(condition => condition.main).OrderByDescending(grp => grp.Count()).First().FirstOrDefault();

            switch (dayWeather.main)
            {
                case "Rain":
                case "Thunderstorm":
                case "Drizzle":
                    return $"Pack an umbrella! It's looking like a {dayWeather.description}. Not the time or place to get wet. Save that for later.";
                case "Snow":
                    return $"Look outside and save your black ass the trouble, we weren't built for this. There is {dayWeather.description} today.";
                case "Fog":
                case "Mist":
                    return $"Make sure you can see where you're going. Don't think about how many horror movies happen in weird weather because there is {dayWeather.description} today.";
                default:
                    return "";
            };
        }

        public async Task<string> GetRandomJoke()
        {
            var jokeMessage = "";
            Joke randomJoke = JsonConvert.DeserializeObject<Joke>(await _httpClient.GetStringAsync(JOKE_API_URL));

            if (randomJoke.joke != null)
                jokeMessage = randomJoke.joke;
            else
                jokeMessage = randomJoke.setup + "\n" + randomJoke.delivery;

            return jokeMessage;
        }

        // TODO: Change the class property names to be caps etc.
        public class ForecastData
        {
            public List<WeatherTimestamp> list { get; set; }
        }

        public class WeatherTimestamp
        {

            [JsonProperty(PropertyName = "dt_txt")]
            public string dt_txt { get; set; }

            [JsonProperty(PropertyName = "main")]
            public JObject main { get; set; }

            [JsonProperty(PropertyName = "weather")]
            public List<JObject> weather { get; set; }
        }

        public class WeatherData
        {
            public string main { get; set; }
            public string description { get; set; }
        }

        public class Joke
        {
            public string type { get; set; }
            public string joke { get; set; }
            public string setup { get; set; }
            public string delivery { get; set; }
            public int id { get; set; }
        }
    }
}
