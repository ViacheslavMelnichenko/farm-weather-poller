using System;
using System.Reflection;
using Farm.Weather.Poller.Options;
using MassTransit.Scheduling;

namespace Farm.Weather.Poller.Scheduler;

public class CollectWeatherDataCommandSchedule : RecurringSchedule
{
    public CollectWeatherDataCommandSchedule(SchedulerOptions.JobInfo jobInfo)
    {
        TimeZoneId = "UTC";
        StartTime = DateTimeOffset.Now;
        EndTime = null;
        ScheduleId = jobInfo.JobId;
        ScheduleGroup = Assembly.GetExecutingAssembly().GetName().Name!;
        CronExpression = jobInfo.CronExpression;
        MisfirePolicy = MissedEventPolicy.Skip;
        Description = "General Schedule";
    }

    public string TimeZoneId { get; }
    public DateTimeOffset StartTime { get; }
    public DateTimeOffset? EndTime { get; }
    public string ScheduleId { get; }
    public string ScheduleGroup { get; }
    public string CronExpression { get; }
    public string Description { get; }
    public MissedEventPolicy MisfirePolicy { get; }
}