using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EarlyConnect.Api.Mappers;
using System.Net;
using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.Application.Commands.CreateMetricData;
using SFA.DAS.EarlyConnect.Application.Queries.GetMetricsDataByLepsCode;

namespace SFA.DAS.EarlyConnect.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/early-connect/metrics-data/")]
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
        [Route("add")]
        public async Task<IActionResult> CreateMetricsData([FromBody] CreateMetricDataPostRequest request)
        {
            try
            {
                await _mediator.Send(new CreateMetricDataCommand
                {
                    metricsData = request.MapFromMetricDataPostRequest()
                });

                return Ok();

            }
            catch (Exception e)
            {
                var errorMessage = (e as SharedOuterApi.Exceptions.ApiResponseException)?.Error;

                _logger.LogError(e, "Error posting Metrics data ");

                return BadRequest($"Error posting Metrics data. {(errorMessage != null ? $"\nErrorInfo: {errorMessage}" : "")}\nMessage: {e.Message}\nStackTrace: {e.StackTrace}");
            }
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("{lepsCode}")]
        public async Task<IActionResult> GetMetricsData([FromRoute] string lepsCode)
        {
            try
            {
                var result = await _mediator.Send(new GetMetricsDataByLepsCodeQuery
                {
                    LepsCode = lepsCode
                });

                return Ok((GetMetricsDataListByLepsCodeResponse)result);
            }
            catch (Exception e)
            {
                var errorMessage = (e as SharedOuterApi.Exceptions.ApiResponseException)?.Error;

                _logger.LogError(e, "Error getting Metrics data ");

                return BadRequest($"Error getting Metrics data. {(errorMessage != null ? $"\nErrorInfo: {errorMessage}" : "")}\nMessage: {e.Message}\nStackTrace: {e.StackTrace}");
            }
        }
    }
}
