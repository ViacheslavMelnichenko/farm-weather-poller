using Farm.Weather.Contracts.Models;
using System;

namespace Farm.Weather.Contracts.IntegrationEvents
{
    public interface IWeatherRawDataCreated
    {
        WeatherGeneralDto WeatherData { get; }
        DateTimeOffset CreateDate { get; }
    }
}