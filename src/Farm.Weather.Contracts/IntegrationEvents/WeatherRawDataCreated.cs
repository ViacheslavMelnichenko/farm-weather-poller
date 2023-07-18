using Farm.Weather.Contracts.Models;
using System;

namespace Farm.Weather.Contracts.IntegrationEvents
{
    public class WeatherRawDataCreated : IWeatherRawDataCreated
    {

        public WeatherRawDataCreated(DateTimeOffset createDate, WeatherGeneralDto weatherData)
        {
            CreateDate = createDate;
            WeatherData = weatherData;
        }

        public WeatherGeneralDto WeatherData { get; }

        public DateTimeOffset CreateDate { get; }
    }

}