using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WorkWithBD;
using Microsoft.OpenApi.Models;

namespace Metrics.Controllers
{
    [Route("api/metric/network")]
    [ApiController]
    public class NetworkController : ControllerBase
    {
        private INetworkMetricsRepository repository;
        private readonly IMapper _mapper;


        public NetworkController(INetworkMetricsRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this._mapper = mapper;
        }



        [HttpPost("create")]
        public IActionResult Create([FromBody] NetworkMetricCreateRequest request)
        {
            repository.Create(new NetworkMetrics
            {
                Time = request.Time,
                Value = request.Value
            });
            return Ok("Created");
        }


        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<NetworkMetrics, NetworkMetricDto>());
            var mapper = config.CreateMapper();
            IList<NetworkMetrics> metrics = repository.GetAll();
            var response = new AllNetworkMetricsResponse()
            {
                Metrics = new List<NetworkMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<NetworkMetricDto>(metric));
            }
            foreach (var metric in metrics)
            {
                response.Metrics.Add(new NetworkMetricDto { Id = metric.Id, Value = metric.Value, Time = metric.Time });
            }
            return Ok(response);
        }


        [HttpPost("delete")]
        public IActionResult Delete([FromQuery] int id)
        {
            repository.Delete(id);
            return Ok("Deleted");
        }


        [HttpPost("update")]
        public IActionResult Update([FromBody] NetworkMetrics metric)
        {
            repository.Update(metric);
            return Ok("Updated");
        }


        [HttpGet("getbyid")]
        public IActionResult GetById([FromQuery] int id)
        {
            var metrics = repository.GetById(id);
            return Ok(metrics);
        }


        [HttpGet("api/metrics/network/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] DateTime fromTime, [FromRoute] DateTime toTime)
        {
            IList<NetworkMetrics> metrics = repository.GetMetricsByTimePeriod(fromTime, toTime);
            List<NetworkMetricDto> Metrics = new List<NetworkMetricDto>();

            foreach (var metric in metrics)
            {
                Metrics.Add(_mapper.Map<NetworkMetricDto>(metric));

            }
            return Ok(Metrics);
        }
    }
}

