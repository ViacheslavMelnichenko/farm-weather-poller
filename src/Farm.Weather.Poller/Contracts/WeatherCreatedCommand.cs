using System;

namespace Farm.Weather.Poller.Contracts
{
    public record WeatherCreatedCommand : IWeatherCreatedCommand
    {
        public string? WeatherData { get; set; }
        public DateTimeOffset CollectionDate { get; set; }
    }
}