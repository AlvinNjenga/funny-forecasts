using FunnyForecasts;

var forecastService = new ForecastService();

var notification = await forecastService.BuildForecastNotification();

// NotificationService.SendSMS(notification);

Console.WriteLine("Finished.");