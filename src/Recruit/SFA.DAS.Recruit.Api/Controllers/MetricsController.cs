using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetVacancyMetrics;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.Recruit.Application.Queries.GetAllVacanciesInMetrics;

namespace SFA.DAS.Recruit.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class MetricsController(IMediator mediator, ILogger<MetricsController> logger) : ControllerBase
    {
        [HttpGet]
        [Route("vacancies/{vacancyReference}")]
        public async Task<IActionResult> GetVacancyMetrics(
            [FromRoute] string vacancyReference,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var queryResult = await mediator.Send(new GetVacancyMetricsQuery(vacancyReference, startDate, endDate));

                return Ok((GetVacancyMetricsResponse)queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting vacancy metrics");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("vacancies")]
        public async Task<IActionResult> GetAllVacanciesInMetrics(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var queryResult = await mediator.Send(new GetAllVacanciesInMetricsQuery(startDate, endDate));

                return Ok((GetAllVacanciesInMetricsApiResponse)queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting all vacancies in metrics");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}