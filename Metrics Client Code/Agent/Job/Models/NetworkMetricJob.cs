using Quartz;
using System.Diagnostics;
using WorkWithBD;
using Microsoft.OpenApi.Models;

namespace MetricsAgent
{
	public class NetworkMetricJob : IJob
	{
		private INetworkMetricsRepository _repository;
		private PerformanceCounter _performanceCounter;
		private string[] inst = new PerformanceCounterCategory("Network Interface").GetInstanceNames();

		public NetworkMetricJob(INetworkMetricsRepository repository)
		{
			_repository = repository;
			_performanceCounter = new PerformanceCounter("Network Interface", "Bytes Total/sec", $"{inst[0]}");
		}

		public Task Execute(IJobExecutionContext context)
		{
			_repository.Create(new Microsoft.OpenApi.Models.NetworkMetrics
			{
				Time = DateTime.Now,
				Value = Convert.ToInt32(_performanceCounter.NextValue())
			});
			return Task.CompletedTask;
		}
	}
}

