using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RecruitJobs.Api.Models;
using SFA.DAS.RecruitJobs.Api.Models.Vacancies.Responses;
using SFA.DAS.RecruitJobs.Domain.Vacancy;
using SFA.DAS.RecruitJobs.GraphQL.RecruitInner.Mappers;
using SFA.DAS.RecruitJobs.InnerApi.Requests.DeleteVacancy;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using StrawberryShake;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using SFA.DAS.RecruitJobs.GraphQL;

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
    public async Task<IResult> GetOne([FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
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
            logger.LogError(e,
                "Unable to get vacancy analytics : An error occurred");
            return Results.Problem(statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPut]
    [Route("{vacancyReference:long}/analytics")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IResult> PutOne([FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromRoute] long vacancyReference, [FromBody] PutVacancyAnalyticsRequest request,
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
            logger.LogError(e,
                "Unable to create vacancy analytics : An error occurred");
            return Results.Problem(statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet, Route("stale/live")]
    [ProducesResponseType(typeof(DataResponse<IEnumerable<VacancyIdentifier>>), StatusCodes.Status200OK)]
    public async Task<IResult> GetVacanciesToClose(
        [FromQuery, Required] DateTime pointInTime,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient
            .GetVacanciesToClose
            .ExecuteAsync(pointInTime, cancellationToken);

        if (!response.IsSuccessResult())
        {
            logger.LogError("An error occured at GetVacanciesToClose: {Errors}", response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var data = response
            .Data!
            .Vacancies
            .Select(x =>
                new VacancyIdentifier(x.Id, x.VacancyReference, VacancyStatus.Live, x.ClosingDate!.Value.UtcDateTime));
        return TypedResults.Ok(new DataResponse<IEnumerable<VacancyIdentifier>>(data));
    }

    [HttpGet, Route("stale/draft")]
    [ProducesResponseType(typeof(DataResponse<IEnumerable<StaleVacancyIdentifier>>), StatusCodes.Status200OK)]
    public async Task<IResult> GetDraftVacanciesToClose(
        [FromQuery, Required] DateTime pointInTime,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient
            .GetDraftVacanciesCreatedBefore
            .ExecuteAsync(pointInTime, cancellationToken);

        if (!response.IsSuccessResult())
        {
            logger.LogError("An error occured at GetDraftVacanciesToClose: {Errors}", response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var data = response
            .Data!
            .Vacancies
            .Select(x =>
                new StaleVacancyIdentifier(x.Id, x.VacancyReference, VacancyStatus.Draft, x.CreatedDate!.Value.UtcDateTime));
        return TypedResults.Ok(new DataResponse<IEnumerable<StaleVacancyIdentifier>>(data));
    }

    [HttpGet, Route("stale/employer-reviewed")]
    [ProducesResponseType(typeof(DataResponse<IEnumerable<StaleVacancyIdentifier>>), StatusCodes.Status200OK)]
    public async Task<IResult> GetEmployerReviewedVacanciesToClose(
        [FromQuery, Required] DateTime pointInTime,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient
            .GetEmployerReviewedVacanciesCreatedBefore
            .ExecuteAsync(pointInTime, cancellationToken);

        if (!response.IsSuccessResult())
        {
            logger.LogError("An error occured at GetEmployerReviewedVacanciesToClose: {Errors}", response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var data = response
            .Data!
            .Vacancies
            .Select(x =>
                new StaleVacancyIdentifier(x.Id, x.VacancyReference, VacancyStatus.Referred, x.CreatedDate!.Value.UtcDateTime));
        return TypedResults.Ok(new DataResponse<IEnumerable<StaleVacancyIdentifier>>(data));
    }

    [HttpGet, Route("stale/rejected")]
    [ProducesResponseType(typeof(DataResponse<IEnumerable<StaleVacancyIdentifier>>), StatusCodes.Status200OK)]
    public async Task<IResult> GetRejectedEmployerVacanciesToClose(
        [FromQuery, Required] DateTime pointInTime,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient
            .GetRejectedEmployerVacanciesCreatedBefore
            .ExecuteAsync(pointInTime, cancellationToken);

        if (!response.IsSuccessResult())
        {
            logger.LogError("An error occured at GetRejectedEmployerVacanciesToClose: {Errors}", response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var data = response
            .Data!
            .Vacancies
            .Select(x =>
                new StaleVacancyIdentifier(x.Id, x.VacancyReference, VacancyStatus.Rejected, x.CreatedDate!.Value.UtcDateTime));
        return TypedResults.Ok(new DataResponse<IEnumerable<StaleVacancyIdentifier>>(data));
    }

    [HttpPost, Route("delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> DeleteOne(
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromBody, Required] Guid id)
    {
        var results = await recruitApiClient.DeleteWithResponseCode<NullResponse>(new DeleteVacancyByIdRequest(id));
        return Results.StatusCode((int)results.StatusCode);
    }
}