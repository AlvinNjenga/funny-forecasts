using FunnyForecasts;

var forecastService = new ForecastService();

var message = await forecastService.GetForecastNotification();

Console.WriteLine(message);
