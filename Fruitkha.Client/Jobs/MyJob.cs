using Fruitkha.Core.Interfaces;
using Fruitkha.Core.Services;

namespace Fruitkha.Client.Jobs;

public class MyJob : CronJobService
{
    private readonly ILogger<MyJob> _logger;

    public MyJob(IScheduleConfig<MyJob> config, ILogger<MyJob> logger) : base(config.CronExpression, config.TimeZoneInfo)
    {
        _logger = logger;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("MyJob starts");
        return base.StartAsync(cancellationToken);
    }

    public override Task DoWork(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{DateTime.Now:hh:mm:ss} MyJob is working");
        return base.DoWork(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("MyJob is stopping");
        return base.StopAsync(cancellationToken);
    }
}