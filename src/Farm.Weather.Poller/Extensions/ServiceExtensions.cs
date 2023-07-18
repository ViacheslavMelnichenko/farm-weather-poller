using System;
using System.Net.Http;
using Farm.Weather.Poller.Clients;
using Farm.Weather.Poller.Consumers;
using Farm.Weather.Poller.Options;
using Farm.Weather.Poller.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Serilog.Core;

namespace Farm.Weather.Poller.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddEventBus(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<CommandBusOptions>(configuration.GetSection(CommandBusOptions.SectionName));
        services.Configure<RabbitMqHostOptions>(configuration.GetSection(RabbitMqHostOptions.RabbitMq));
        services.Configure<SchedulerOptions>(configuration.GetSection(SchedulerOptions.SectionName));

        services.AddSingleton<IEventPublisher, EventPublisher>();

        services
            .AddMassTransit(x =>
            {
                var commandBusOptions = configuration.GetSection(CommandBusOptions.SectionName)
                    .Get<CommandBusOptions>()!;

                x.SetKebabCaseEndpointNameFormatter();
                x.AddDelayedMessageScheduler();
                x.AddConsumer<FarmCollectWeatherDataCommandConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitOptions = configuration.GetSection(RabbitMqHostOptions.RabbitMq)
                        .Get<RabbitMqHostOptions>()!;

                    var schedulerOptions = configuration.GetSection(SchedulerOptions.SectionName)
                        .Get<SchedulerOptions>()!;

                    cfg.Host(rabbitOptions.Host, rabbitOptions.VirtualHost, h =>
                    {
                        h.Username(rabbitOptions.UserName);
                        h.Password(rabbitOptions.Password);
                    });

                    cfg.ReceiveEndpoint(commandBusOptions.WeatherDataQueue, e =>
                    {
                        e.PrefetchCount = 1;
                    
                        e.ConfigureConsumer<FarmCollectWeatherDataCommandConsumer>(context);
                    });

                    if (schedulerOptions.UseInMemoryScheduler)
                    {
                        cfg.UseInMemoryScheduler(schedulerOptions.QueueName);
                    }

                    cfg.UseDefaultRetrySettings();
                    cfg.ConfigureEndpoints(context);
                });
            });

        return services;
    }

    public static IServiceCollection AddCustomLogging(this IServiceCollection services,
        HostBuilderContext hostBuilderContext)
    {
        var levelSwitch = new LoggingLevelSwitch();

        if (!hostBuilderContext.HostingEnvironment.IsDevelopment())
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel
                .Override("Microsoft", levelSwitch)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(
                    serverUrl: hostBuilderContext.Configuration["Seq:Url"]!,
                    apiKey: hostBuilderContext.Configuration["Seq:ApiKey"])
                .Enrich.WithProperty("Service", typeof(Program).Assembly.GetName().Name!)
                .Enrich.WithProperty("Environment", hostBuilderContext.HostingEnvironment.EnvironmentName)
                .CreateLogger();
        }
        else
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel
                .Override("Microsoft", levelSwitch)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .Enrich.WithProperty("Service", typeof(Program).Assembly.GetName().Name!)
                .Enrich.WithProperty("Environment", hostBuilderContext.HostingEnvironment.EnvironmentName)
                .CreateLogger();
        }

        return services;
    }

    public static IServiceCollection AddWeatherClient(this IServiceCollection services)
    {
        services
            .AddHttpClient<IWeatherClient>()
            .AddPolicyHandler(GetHttpClientRetryPolicy());
        services.AddSingleton<IWeatherClient, WeatherClient>();
        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetHttpClientRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static void UseDefaultRetrySettings(this IConsumePipeConfigurator cpc)
    {
        cpc.UseMessageRetry(r =>
        {
            r.Ignore<ArgumentException>();
            r.Immediate(retryLimit: 2);
        });
    }
}