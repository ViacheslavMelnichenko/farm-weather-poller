using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Farm.Weather.Contracts.Common;
using Farm.Weather.Contracts.IntegrationEvents;
using Farm.Weather.Contracts.Models;
using Farm.Weather.Poller.Clients;
using Farm.Weather.Poller.Commands;
using Farm.Weather.Poller.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Farm.Weather.Poller.Consumers;

public class FarmCollectWeatherDataCommandConsumer : IConsumer<IFarmCollectWeatherDataCommand>
{
    private readonly IWeatherClient _weatherClient;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<FarmCollectWeatherDataCommandConsumer> _logger;
    private const int ApiChunkSize = 5;
    private const int IntegrationCommandsChunkSize = 10;

    public FarmCollectWeatherDataCommandConsumer(
        IWeatherClient weatherClient,
        IEventPublisher eventPublisher,
        ILogger<FarmCollectWeatherDataCommandConsumer> logger)
    {
        _weatherClient = weatherClient;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IFarmCollectWeatherDataCommand> context)
    {
        var nextScheduled = context.GetQuartzNextScheduled();
        if (nextScheduled.HasValue && nextScheduled < DateTime.UtcNow)
        {
            return;
        }

        _logger.LogInformation("Collect weather data for {Cities}", Constants.UkrainianCities);

        var weatherData = new List<WeatherGeneralDto>();

        foreach (var cities in Constants.UkrainianCities.Chunk(ApiChunkSize))
        {
            var tasks = cities.Select(c => _weatherClient.GetCityCurrentWeatherAsync(c)).ToList();
            await Task.WhenAll(tasks);
            weatherData.AddRange(tasks.Where(r => r.Result != null).Select(x => x.Result)!);
        }

        var integrationCommands = weatherData
            .Select(data => new WeatherRawDataCreated(DateTimeOffset.UtcNow, data))
            .ToList();

        foreach (IWeatherRawDataCreated[] commandsChunk in integrationCommands.Chunk(IntegrationCommandsChunkSize))
        {
            await _eventPublisher.SendIntegrationCommandAsync(commandsChunk);
        }

        _logger.LogInformation("Weather data collected.");
    }
}