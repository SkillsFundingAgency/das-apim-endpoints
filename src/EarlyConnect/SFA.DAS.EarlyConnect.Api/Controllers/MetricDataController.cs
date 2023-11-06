using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EarlyConnect.Api.Mappers;
using System.Net;
using SFA.DAS.EarlyConnect.Application.Commands.MetricData;
using SFA.DAS.EarlyConnect.Api.Models;

namespace SFA.DAS.EarlyConnect.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/metrics-data/")]
    public class MetricDataController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MetricDataController> _logger;

        public MetricDataController(IMediator mediator, ILogger<MetricDataController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("")]
        public async Task<IActionResult> Post([FromBody] CreateMetricDataPostRequest request)
        {
            try
            {
                await _mediator.Send(new CreateMetricDataCommand
                {
                    MetricDataList = request.MapFromMetricDataPostRequest()
                });

                return Ok();

           }
            catch (Exception e)
            {
                _logger.LogError(e, "Error posting Metrics data");
                return BadRequest();
            }
        }
    }
}
