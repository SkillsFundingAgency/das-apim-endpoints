using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.RecruitJobs.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class VacanciesController(ILogger<VacanciesController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{vacancyReference:long}/analytics")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GetOneVacancyAnalyticsResponse), StatusCodes.Status200OK)]
    public async Task<IResult> GetOne(
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromRoute] long vacancyReference,
        CancellationToken token = default)
    {
        try
        {
            logger.LogInformation("Recruit API: Received request to get vacancy analytics for vacancy reference: {VacancyReference}", vacancyReference);

            var result = await recruitApiClient.Get<GetOneVacancyAnalyticsResponse>(new GetOneVacancyAnalyticsApiRequest(vacancyReference));

            var response = result ?? new GetOneVacancyAnalyticsResponse
            {
                VacancyReference = vacancyReference,
                Analytics = []
            };

            response.Analytics ??= [];

            return TypedResults.Ok(response);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to get vacancy analytics : An error occurred");
            return Results.Problem(statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPut]
    [Route("{vacancyReference:long}/analytics")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IResult> PutOne(
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromRoute] long vacancyReference,
        [FromBody] PutVacancyAnalyticsRequest request,
        CancellationToken token = default)
    {
        try
        {
            logger.LogInformation("Recruit API: Received request to create vacancy analytics for vacancy reference: {VacancyReference}", vacancyReference);

            await recruitApiClient.Put(new PutOneVacancyAnalyticsApiRequest(vacancyReference, new PutOneVacancyAnalyticsApiRequest.VacancyAnalyticsRequestData
            {
                AnalyticsData = request.AnalyticsData
            }));

            return TypedResults.Created();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to create vacancy analytics : An error occurred");
            return Results.Problem(statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }
}
