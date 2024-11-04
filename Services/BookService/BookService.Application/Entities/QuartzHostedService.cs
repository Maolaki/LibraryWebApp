using LibraryWebApp.BookService.Application.Entities;
using Microsoft.Extensions.Hosting;
using Quartz;

public class QuartzHostedService : IHostedService
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IEnumerable<JobSchedule> _jobSchedules;
    private IScheduler? _scheduler;

    public QuartzHostedService(ISchedulerFactory schedulerFactory, IEnumerable<JobSchedule> jobSchedules)
    {
        _schedulerFactory = schedulerFactory;
        _jobSchedules = jobSchedules;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        foreach (var jobSchedule in _jobSchedules)
        {
            var job = CreateJob(jobSchedule.JobType);
            var trigger = CreateTrigger(jobSchedule.CronExpression);
            await _scheduler.ScheduleJob(job, trigger, cancellationToken);
        }
        await _scheduler.Start(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_scheduler != null)
        {
            await _scheduler.Shutdown(cancellationToken);
        }
    }

    private IJobDetail CreateJob(Type jobType)
    {
        string jobName = jobType.FullName ?? throw new ArgumentNullException(nameof(jobType.FullName));
        return JobBuilder.Create(jobType)
            .WithIdentity(jobName)
            .Build();
    }

    private ITrigger CreateTrigger(string cronExpression)
    {
        return TriggerBuilder.Create()
            .WithIdentity($"{cronExpression}-trigger")
            .WithCronSchedule(cronExpression)
            .Build();
    }
}