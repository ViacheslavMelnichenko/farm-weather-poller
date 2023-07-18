using Farm.Weather.Contracts.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Farm.Weather.Poller.Clients;

public interface IWeatherClient
{
    Task<WeatherGeneralDto?> GetCityCurrentWeatherAsync(string city, CancellationToken token = default);
}