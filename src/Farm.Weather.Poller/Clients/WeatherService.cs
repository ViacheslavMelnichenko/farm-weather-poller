using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Farm.Weather.Poller.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Farm.Weather.Poller.Clients
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherService> _logger;
        private readonly WeatherApiOptions _apiOptions;

        public WeatherService(HttpClient httpClient, IOptions<WeatherApiOptions> apiOptions,
            ILogger<WeatherService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException();
            _logger = logger;
            _apiOptions = apiOptions.Value;
            _httpClient.BaseAddress = new Uri(_apiOptions.Url);
        }

        public async Task<string?> GetCityCurrentWeatherAsync(string city, CancellationToken token = default)
        {
            _logger.LogInformation("Current options are: {@ApiOptions}", _apiOptions);

            var query = $"v1/current.json?q={city}&key={_apiOptions.Key}&aqi=yes";
            return await _httpClient.GetStringAsync(query, token);
        }
    }
}