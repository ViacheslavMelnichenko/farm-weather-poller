namespace Farm.Weather.Poller.Options;

public class WeatherApiOptions
{
    public const string WeatherApi = "WeatherApi";

    public string ApiKey { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}