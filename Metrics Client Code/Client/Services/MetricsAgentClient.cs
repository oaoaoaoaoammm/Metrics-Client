using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Metrics_app.Services
{
    public class MetricsAgentClient : IMetricsAgentClient
    {
        private readonly HttpClient _httpClient;


        public MetricsAgentClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<CpuMetrics> GetCpuMetrics(GetAllCpuMetricsApiRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metric/cpu/getbyid?id={request.id}");
            try
            {
                HttpResponseMessage httpResponseMessage = _httpClient.SendAsync(httpRequest).Result;
                string response = await httpResponseMessage.Content.ReadAsStringAsync();
                return (CpuMetrics)JsonConvert.DeserializeObject(response, typeof(CpuMetrics));
            }
            catch (Exception ex)
            {
            }
            return null;
        }


        public async Task<NetworkMetrics> GetNetworkMetrics(GetAllNetWorkTrafficMetricsApiRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metric/network/getbyid?id={request.id}");
            try
            {
                HttpResponseMessage httpResponseMessage = _httpClient.SendAsync(httpRequest).Result;
                string response = await httpResponseMessage.Content.ReadAsStringAsync();
                return (NetworkMetrics)JsonConvert.DeserializeObject(response, typeof(NetworkMetrics));
            }
            catch (Exception ex)
            {
            }
            return null;
        }


        public async Task<HddMetrics> GetHddMetrics(GetAllHddMetricsApiRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metric/hdd/getbyid?id={request.id}");
            try
            {
                HttpResponseMessage httpResponseMessage = _httpClient.SendAsync(httpRequest).Result;
                string response = await httpResponseMessage.Content.ReadAsStringAsync();
                return (HddMetrics)JsonConvert.DeserializeObject(response, typeof(HddMetrics));
            }
            catch (Exception ex)
            {
            }
            return null;
        }


        public async Task<DotNetMetrics> GetDotnetMetrics(DonNetHeapMetrisApiRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metric/dotnet/getbyid?id={request.id}");
            try
            {
                HttpResponseMessage httpResponseMessage = _httpClient.SendAsync(httpRequest).Result;
                string response = await httpResponseMessage.Content.ReadAsStringAsync();
                return (DotNetMetrics)JsonConvert.DeserializeObject(response, typeof(DotNetMetrics));
            }
            catch (Exception ex)
            {
            }
            return null;
        }


        public async Task<RamMetrics> GetRamMetrics(GetAllRamMetricsApiRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metric/ram/getbyid?id={request.id}");
            try
            {
                HttpResponseMessage httpResponseMessage = _httpClient.SendAsync(httpRequest).Result;
                string response = await httpResponseMessage.Content.ReadAsStringAsync();
                return (RamMetrics)JsonConvert.DeserializeObject(response, typeof(RamMetrics));
            }
            catch (Exception ex)
            {
            }
            return null;
        }

    }
}
