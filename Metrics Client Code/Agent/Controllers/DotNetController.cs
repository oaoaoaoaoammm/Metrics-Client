using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WorkWithBD;
using Microsoft.OpenApi.Models;

namespace Metrics.Controllers
{
    [Route("api/metric/dotnet")]
    [ApiController]
    public class DotNetController : ControllerBase
    {
        private IDotNetMetricsRepository repository;
        private readonly IMapper _mapper;


        public DotNetController( IDotNetMetricsRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this._mapper = mapper;
        }



        [HttpPost("create")]
        public IActionResult Create([FromBody] DotNetMetricCreateRequest request)
        {
            repository.Create(new DotNetMetrics
            {
                Time = request.Time,
                Value = request.Value
            });
            return Ok("Created");
        }


        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DotNetMetrics, DotNetDto>());
            var mapper = config.CreateMapper();
            IList<DotNetMetrics> metrics = repository.GetAll();
            var response = new AllDotNetMetricsResponse
            {
                Metrics = new List<DotNetDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<DotNetDto>(metric));
            }
            foreach (var metric in metrics)
            {
                response.Metrics.Add(new DotNetDto { Id = metric.Id, Value = metric.Value, Time = metric.Time });
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
        public IActionResult Update([FromBody] DotNetMetrics metric)
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


        [HttpGet("api/metrics/dotnet/errors-count/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent( [FromRoute] DateTime fromTime, [FromRoute] DateTime toTime)
        {
            IList<DotNetMetrics> metrics = repository.GetMetricsByTimePeriod(fromTime, toTime);
            List<DotNetDto> Metrics = new List<DotNetDto>();

            foreach (var metric in metrics)
            {
                Metrics.Add(_mapper.Map<DotNetDto>(metric));

            }
            return Ok(Metrics);
        }
    }
}

