using System.Threading;
using System.Threading.Tasks;

namespace Farm.Weather.Poller.Clients
{
    public interface IWeatherService
    {
        Task<string?> GetCityCurrentWeatherAsync(string city, CancellationToken token = default);
    }
}