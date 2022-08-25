using System;
using Farm.Weather.Poller.Contracts;
using Farm.Weather.Poller.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;

namespace Farm.Weather.Poller.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomEventBus(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((host, cfg) =>
                {
                    var rabbitOptions = configuration.GetSection(RabbitOptions.RabbitMq)
                        .Get<RabbitOptions>();

                    var weatherBusOptions = configuration.GetSection(WeatherBusOptions.WeatherCommandBus)
                        .Get<WeatherBusOptions>();

                    cfg.Host(rabbitOptions.Url, rabbitOptions.VirtualHost, h =>
                    {
                        h.Username(rabbitOptions.UserName);
                        h.Password(rabbitOptions.Password);
                    });

                    var weatherBusUri = new Uri($"queue:{weatherBusOptions.QueueName}");
                    EndpointConvention.Map<IWeatherCreatedCommand>(weatherBusUri);

                    cfg.ConfigureEndpoints(host);
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
                        serverUrl: hostBuilderContext.Configuration["SeqApi:Url"],
                        apiKey: hostBuilderContext.Configuration["SeqApi:Key"])
                    .Enrich.WithProperty("Service", typeof(Program).Assembly.GetName().Name!)
                    .Enrich.WithProperty("Environment", hostBuilderContext.HostingEnvironment.EnvironmentName)
                    .CreateLogger();
            }

            return services;
        }

        private static void UseDefaultRetrySettings(this IConsumePipeConfigurator cpc)
        {
            cpc.UseMessageRetry(
                r =>
                {
                    r.Ignore<ArgumentException>();
                    r.Immediate(2);
                });
        }
    }
}