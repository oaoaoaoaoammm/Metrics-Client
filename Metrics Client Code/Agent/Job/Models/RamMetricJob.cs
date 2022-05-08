using Quartz;
using System.Diagnostics;
using WorkWithBD;

namespace MetricsAgent
{
	public class RamMetricJob : IJob
	{
		private IRamMetricsRepository _repository;
		private PerformanceCounter _performanceCounter;

		public RamMetricJob(IRamMetricsRepository repository)
		{
			_repository = repository;
			_performanceCounter = new PerformanceCounter("Memory", "Available MBytes");
		}

		public Task Execute(IJobExecutionContext context)
		{
			_repository.Create(new Microsoft.OpenApi.Models.RamMetrics
			{
				Time = DateTime.Now,
				Value = Convert.ToInt32(_performanceCounter.NextValue())
			});
			return Task.CompletedTask;
		}
	}
}

