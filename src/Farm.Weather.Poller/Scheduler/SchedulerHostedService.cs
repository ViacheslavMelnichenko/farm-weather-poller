using System;
using System.Threading;
using System.Threading.Tasks;
using Farm.Weather.Poller.Options;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Farm.Weather.Poller.Scheduler;

public class SchedulerHostedService : IHostedService
{
    private readonly IBus _bus;
    private readonly RabbitMqHostOptions _rabbitOptions;
    private readonly CommandBusOptions _commandBusOptions;
    private readonly SchedulerOptions _schOptions;

    public SchedulerHostedService(IBus bus,
                                  IOptions<RabbitMqHostOptions> rabbitOptions,
                                  IOptions<CommandBusOptions> commandBusOptions,
                                  IOptions<SchedulerOptions> schOptions)
    {
        _bus = bus;
        _rabbitOptions = rabbitOptions.Value;
        _commandBusOptions = commandBusOptions.Value;
        _schOptions = schOptions.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Uri inputQueueAddress =
            new($"{_rabbitOptions.Url}/{_rabbitOptions.VirtualHost}/{_commandBusOptions.WeatherDataQueue}");

        await _bus.ScheduleRecurringSend(
            inputQueueAddress,
            new CollectWeatherDataCommandSchedule(_schOptions.CollectWeatherData),
            new FarmCollectWeatherDataCommand(),
            cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}