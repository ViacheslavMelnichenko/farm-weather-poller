using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Farm.Weather.Contracts.Models;
using Farm.Weather.Poller.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Farm.Weather.Poller.Clients;

public class WeatherClient : IWeatherClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WeatherClient> _logger;
    private readonly WeatherApiOptions _apiOptions;

    public WeatherClient(HttpClient httpClient,
        IOptions<WeatherApiOptions> apiOptions,
        ILogger<WeatherClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException();
        _logger = logger;
        _apiOptions = apiOptions.Value;
        _httpClient.BaseAddress = new Uri(_apiOptions.Url);
    }

    public async Task<WeatherGeneralDto?> GetCityCurrentWeatherAsync(string city, CancellationToken token = default)
    {
        _logger.LogInformation("Current options are: {@ApiOptions}", _apiOptions);

        var query = $"v1/current.json?q={city}&key={_apiOptions.ApiKey}&aqi=yes";
        var result = await _httpClient.GetFromJsonAsync<WeatherGeneralDto?>(query, token);
        return result;
    }
}