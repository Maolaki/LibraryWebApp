using Quartz.Spi;
using Quartz;

namespace LibraryWebApp.BookService.Application.Entities
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var job = _serviceProvider.GetService(bundle.JobDetail.JobType);

            if (job is null)
            {
                throw new InvalidOperationException($"Cannot create instance of job type {bundle.JobDetail.JobType}");
            }

            return (IJob)job;
        }

        public void ReturnJob(IJob job)
        {
            throw new NotImplementedException();
        }
    }
}
