using Farm.Weather.Poller;
using Farm.Weather.Poller.Clients;
using Farm.Weather.Poller.Configuration;
using Farm.Weather.Poller.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Farm.Weather.Poller.Options;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Add(new VaultConfigurationSource(hostContext.HostingEnvironment.EnvironmentName));

        hostContext.Configuration = builder.Build();

        services.AddCustomLogging(hostContext);
        services.AddCustomEventBus(hostContext.Configuration);

        services.Configure<WeatherApiOptions>(hostContext.Configuration.GetSection(WeatherApiOptions.WeatherApi));

        services.AddHttpClient<IWeatherService>();
        services.AddSingleton<IWeatherService, WeatherService>();
        services.AddHostedService<Worker>();
    })
    .UseSerilog()
    .Build();

await host.RunAsync();