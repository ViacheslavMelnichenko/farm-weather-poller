namespace Farm.Weather.Poller.Options;

public class SchedulerOptions
{
    public const string SectionName = "Scheduler";

    public string QueueName { get; set; } = null!;

    public bool UseInMemoryScheduler { get; set; }

    public JobInfo CollectWeatherData { get; set; } = null!;

    public class JobInfo
    {
        public string JobId { get; set; } = null!;

        public string CronExpression { get; set; } = null!;
    }
}