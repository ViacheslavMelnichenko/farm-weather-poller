namespace Farm.Weather.Poller.Options
{
    public class WeatherBusOptions
    {
        public const string WeatherCommandBus = "WeatherCommandBus";

        public string QueueName { get; set; } = null!;
    }
}