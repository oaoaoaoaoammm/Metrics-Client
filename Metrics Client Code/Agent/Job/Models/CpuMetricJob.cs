using Quartz;
using System.Diagnostics;
using WorkWithBD;

namespace MetricsAgent
{
	public class CpuMetricJob : IJob
	{
		private ICpuMetricsRepository _repository;
        private PerformanceCounter _performanceCounter;

        public CpuMetricJob(ICpuMetricsRepository repository)
		{
			_repository = repository;
            _performanceCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            _repository.Create(new Microsoft.OpenApi.Models.CpuMetrics
            {
                Time = DateTime.Now,
                Value = Convert.ToInt32(_performanceCounter.NextValue())
            });
            return Task.CompletedTask;
        }
    }
}

