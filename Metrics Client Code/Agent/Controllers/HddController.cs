using AutoMapper;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using WorkWithBD;

namespace Metrics.Controllers
{
    [Route("api/metric/hdd")]
    [ApiController]
    public class HddController : ControllerBase
    {
        private IHddMetricsRepository repository;
        private readonly IMapper _mapper;


        public HddController(IHddMetricsRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this._mapper = mapper;
        }



        [HttpPost("create")]
        public IActionResult Create([FromBody] HddMetricCreateRequest request)
        {
            repository.Create(new HddMetrics
            {
                Time = request.Time,
                Value = request.Value
            });
            return Ok("Created");
        }


        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<HddMetrics, HddMetricDto>());
            var mapper = config.CreateMapper();
            IList<HddMetrics> metrics = repository.GetAll();
            var response = new AllHddMetricsResponse()
            {
                Metrics = new List<HddMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<HddMetricDto>(metric));
            }
            foreach (var metric in metrics)
            {
                response.Metrics.Add(new HddMetricDto { Id = metric.Id, Value = metric.Value, Time = metric.Time });
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
        public IActionResult Update([FromBody] HddMetrics metric)
        {
            repository.Update(metric);
            return Ok("Updated");
        }


        [HttpPost("api/metric/hdd/from/{fromTime}/to/{toTime}")]
        public IActionResult GetById([FromRoute] DateTime fromTime, [FromRoute] DateTime toTime)
        {
            IList<HddMetrics> metrics = repository.GetMetricsByTimePeriod(fromTime, toTime);
            List<HddMetricDto> Metrics = new List<HddMetricDto>();

            foreach (var metric in metrics)
            {
                Metrics.Add(_mapper.Map<HddMetricDto>(metric));

            }
            return Ok(Metrics);
        }


        [HttpGet("getbyid")]
        public IActionResult GetMetricsFromAgent([FromQuery] int id)
        {
            var metrics = repository.GetById(id);
            return Ok(metrics);
        }
    }
}

