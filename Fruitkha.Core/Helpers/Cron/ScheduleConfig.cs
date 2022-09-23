using Fruitkha.Core.Interfaces;

namespace Fruitkha.Core.Helpers.Cron;

public class ScheduleConfig<T> : IScheduleConfig<T>
{
    public string CronExpression { get; set; }
    public TimeZoneInfo TimeZoneInfo { get; set; }
}