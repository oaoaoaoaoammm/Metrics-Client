using Quartz;
using System.Diagnostics;
using WorkWithBD;

namespace MetricsAgent
{
	public class HddMetricJob : IJob
	{
		private IHddMetricsRepository _repository;
		private PerformanceCounter _performanceCounter;

		public HddMetricJob(IHddMetricsRepository repository)
		{
			_repository = repository;
			_performanceCounter = new PerformanceCounter("PhysicalDisk", "Disk Reads/sec", "_Total");
		}

		public Task Execute(IJobExecutionContext context)
		{
			_repository.Create(new Microsoft.OpenApi.Models.HddMetrics
			{
				Time = DateTime.Now,
				Value = Convert.ToInt32(_performanceCounter.NextValue())
			});
			return Task.CompletedTask;
		}
	}
}

