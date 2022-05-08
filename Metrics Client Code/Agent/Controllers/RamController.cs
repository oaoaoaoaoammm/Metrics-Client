using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WorkWithBD;
using Microsoft.OpenApi.Models;

namespace Metrics.Controllers
{
    [Route("api/metric/ram")]
    [ApiController]
    public class RamController : ControllerBase
    {
        private IRamMetricsRepository repository;
        private readonly IMapper _mapper;


        public RamController(IRamMetricsRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this._mapper = mapper;
        }



        [HttpPost("create")]
        public IActionResult Create([FromBody] RamMetricCreateRequest request)
        {
            repository.Create(new RamMetrics
            {
                Time = request.Time,
                Value = request.Value
            });
            return Ok("Created");
        }


        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RamMetrics, RamMetricDto>());
            var mapper = config.CreateMapper();
            IList<RamMetrics> metrics = repository.GetAll();
            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<RamMetricDto>(metric));
            }
            foreach (var metric in metrics)
            {
                response.Metrics.Add(new RamMetricDto { Id = metric.Id, Value = metric.Value, Time = metric.Time });
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
        public IActionResult Update([FromBody] RamMetrics metric)
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


        [HttpGet("api/metrics/ram/available/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] DateTime fromTime, [FromRoute] DateTime toTime)
        {
            IList<RamMetrics> metrics = repository.GetMetricsByTimePeriod(fromTime, toTime);
            List<RamMetricDto> Metrics = new List<RamMetricDto>();

            foreach (var metric in metrics)
            {
                Metrics.Add(_mapper.Map<RamMetricDto>(metric));

            }
            return Ok(Metrics);
        }
    }
}

