using Farm.Weather.Poller.Configuration;
using Farm.Weather.Poller.Extensions;
using Farm.Weather.Poller.Options;
using Farm.Weather.Poller.Scheduler;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
               .ConfigureServices((hostContext, services) =>
               {
                   var builder = new ConfigurationBuilder()
                                 .SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                 .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                                              optional: true)
                                 .AddEnvironmentVariables()
                                 .Add(new VaultConfigurationSource(hostContext.HostingEnvironment.EnvironmentName));

                   hostContext.Configuration = builder.Build();

                   services.Configure<WeatherApiOptions>(
                       hostContext.Configuration.GetSection(WeatherApiOptions.WeatherApi));

                   services
                       .AddCustomLogging(hostContext)
                       .AddWeatherClient()
                       .AddEventBus(hostContext.Configuration)
                       .AddHostedService<SchedulerHostedService>();
               })
               .UseSerilog()
               .Build();

await host.RunAsync();