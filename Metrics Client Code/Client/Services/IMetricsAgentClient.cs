using Microsoft.OpenApi.Models;
using System.Threading.Tasks;

namespace Metrics_app.Services
{
    public interface IMetricsAgentClient
    {
        Task<CpuMetrics> GetCpuMetrics(GetAllCpuMetricsApiRequest request);
        Task<NetworkMetrics> GetNetworkMetrics(GetAllNetWorkTrafficMetricsApiRequest request);
        Task<HddMetrics> GetHddMetrics(GetAllHddMetricsApiRequest request);
        Task<DotNetMetrics> GetDotnetMetrics(DonNetHeapMetrisApiRequest request);
        Task<RamMetrics> GetRamMetrics(GetAllRamMetricsApiRequest request);
    }
}
