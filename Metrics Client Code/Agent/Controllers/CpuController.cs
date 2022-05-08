using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WorkWithBD;
using Microsoft.OpenApi.Models;

namespace Metrics.Controllers
{
    [Route("api/metric/cpu")]
    [ApiController]
    public class CpuController : ControllerBase
    {
        private ICpuMetricsRepository _repository;
        private readonly IMapper _mapper;


        public CpuController(ICpuMetricsRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }



        [HttpPost("create")]
        public IActionResult Create([FromBody] CpuMetricCreateRequest request)
        {
            _repository.Create(new CpuMetrics
            {
                Time = request.Time,
                Value = request.Value
            });
            return Ok("Created");
        }


        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CpuMetrics, CpuMetricDto>());
            var mapper = config.CreateMapper();
            IList<CpuMetrics> metrics = _repository.GetAll();
            var response = new AllCpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<CpuMetricDto>(metric));
            }
            foreach (var metric in metrics)
            {
                response.Metrics.Add(new CpuMetricDto { Id = metric.Id, Value = metric.Value, Time = metric.Time });
            }
            return Ok(response);
        }


        [HttpPost("delete")]
        public IActionResult Delete([FromQuery] int id)
        {
             _repository.Delete(id);
            return Ok("Deleted");
        }


        [HttpPost("update")]
        public IActionResult Update([FromBody] CpuMetrics metric)
        {
            _repository.Update(metric);
            return Ok("Updated");
        }


        [HttpGet("getbyid")]
        public IActionResult GetById([FromQuery] int id)
        {
            var metrics = _repository.GetById(id);
            return Ok(metrics);
        }


        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] DateTime fromTime, [FromRoute] DateTime toTime)
        {
            IList<CpuMetrics> metrics = _repository.GetMetricsByTimePeriod(fromTime, toTime);
            List<CpuMetricDto> Metrics = new List<CpuMetricDto>();

            foreach (var metric in metrics)
            {
                Metrics.Add(_mapper.Map<CpuMetricDto>(metric));

            }
            return Ok(Metrics);
        }
    }
}

