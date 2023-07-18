namespace Farm.Weather.Poller.Options;

public class CommandBusOptions
{
    public const string SectionName = "CommandBus";

    public string WeatherDataQueue { get; set; } = null!;
}