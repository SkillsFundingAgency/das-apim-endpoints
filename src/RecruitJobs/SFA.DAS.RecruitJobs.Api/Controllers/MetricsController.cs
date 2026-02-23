using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitJobs.InnerApi.Requests.VacancyMetrics;
using SFA.DAS.RecruitJobs.InnerApi.Responses.VacancyMetrics;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.RecruitJobs.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class MetricsController : ControllerBase
{
    [HttpGet]
    [Route("vacancies")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GetVacancyMetricsByDateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> GetByDate(
        [FromServices] IBusinessMetricsApiClient<BusinessMetricsConfiguration> businessMetricsApiClient,
        [FromQuery, Required] DateTime startDate,
        [FromQuery, Required] DateTime endDate)
    {
        var results = await businessMetricsApiClient.Get<GetVacancyMetricsByDateResponse>(new GetVacancyMetricsByDateRequest(startDate, endDate));
        return TypedResults.Ok(results ?? new GetVacancyMetricsByDateResponse());
    }
}