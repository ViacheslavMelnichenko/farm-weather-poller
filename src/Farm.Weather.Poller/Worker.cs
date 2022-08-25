using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Farm.Weather.Poller.Clients;
using Farm.Weather.Poller.Contracts;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Farm.Weather.Poller
{
    public class Worker : BackgroundService
    {
        private readonly IBus _bus;
        private readonly IWeatherService _weatherService;
        private readonly ILogger<Worker> _logger;
        private const int DelayMinutes = 10;
        private const int ChunkSize = 2;

        public Worker(IBus bus, IWeatherService weatherService, ILogger<Worker> logger)
        {
            _bus = bus;
            _weatherService = weatherService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Poller. Starting Poller");
            var publishedMessages = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                var weatherData = new List<string>();
                var endpoint =
                    await _bus.GetSendEndpoint(new Uri($"exchange:{typeof(IWeatherCreatedCommand).FullName}"));
                foreach (var cities in Constants.Constants.UkrainianCities.Chunk(ChunkSize))
                {
                    var tasks = cities.Select(c => _weatherService.GetCityCurrentWeatherAsync(c, stoppingToken))
                        .ToList();
                    await Task.WhenAll(tasks);
                    weatherData.AddRange(tasks.Select(x => x.Result)!);
                }

                foreach (var weather in weatherData.Chunk(ChunkSize))
                {
                    var tasks = weather
                        .Select(w => new WeatherCreatedCommand { WeatherData = w, CollectionDate = DateTimeOffset.Now })
                        .Select(async w => await endpoint.Send(w, stoppingToken))
                        .ToList();

                    await Task.WhenAll(tasks);
                    publishedMessages += tasks.Count;
                    _logger.LogDebug("Published {PublishedMessages} out of {Count} messages", publishedMessages,
                        weatherData.Count);
                }

                await Task.Delay(TimeSpan.FromMinutes(DelayMinutes), stoppingToken);
            }

            _logger.LogInformation("Poller. Stopping Poller");
        }
    }
}