using FunnyForecasts;

var forecastService = new ForecastService();

var data = await forecastService.GetRandomApiCall();

Console.WriteLine(data);
