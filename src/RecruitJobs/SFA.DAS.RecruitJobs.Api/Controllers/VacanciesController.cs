using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RecruitJobs.Api.Models;
using SFA.DAS.RecruitJobs.Api.Models.Mappers;
using SFA.DAS.RecruitJobs.Api.Models.Requests;
using SFA.DAS.RecruitJobs.Api.Models.Vacancies.Responses;
using SFA.DAS.RecruitJobs.Domain.Vacancy;
using SFA.DAS.RecruitJobs.GraphQL;
using SFA.DAS.RecruitJobs.GraphQL.RecruitInner.Mappers;
using SFA.DAS.RecruitJobs.InnerApi.Requests.Vacancy;
using SFA.DAS.RecruitJobs.InnerApi.Requests.VacancyAnalytics;
using SFA.DAS.RecruitJobs.InnerApi.Responses.VacancyAnalytics;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using StrawberryShake;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using SFA.DAS.RecruitJobs.InnerApi.Responses.Vacancy;
using SFA.DAS.SharedOuterApi.Extensions;
using ClosureReason = SFA.DAS.RecruitJobs.Domain.Vacancy.ClosureReason;
using VacancyStatus = SFA.DAS.RecruitJobs.Domain.Vacancy.VacancyStatus;

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

    [HttpGet, Route("getVacanciesToClose")]
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

    [HttpPost, Route("close")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IResult> CloseVacancy(
        [FromBody] CloseVacancyRequest request,
        [FromServices] VacancyMapper vacancyMapper,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await recruitGqlClient
                .GetVacancyById
                .ExecuteAsync(request.VacancyId, cancellationToken);
            
            if (!response.IsSuccessResult())
            {
                logger.LogError("An error occured at CloseVacancy: {Errors}", response.FormatErrors());
                return TypedResults.Problem(response.ToProblemDetails());
            }

            var vacancy = response.Data!.Vacancies.FirstOrDefault();
            if (vacancy is null)
            {
                logger.LogWarning("Vacancy with id {VacancyId} not found at CloseVacancy", request.VacancyId);
                return TypedResults.NotFound();
            }

            var domainVacancy = GqlVacancyMapper.From(vacancy);
            domainVacancy.ClosureReason = request.ClosureReason;
            domainVacancy.Status = VacancyStatus.Closed;
            domainVacancy.ClosedDate = DateTime.UtcNow;

            var putResponse = await recruitApiClient.PutWithResponseCode<PutVacancyResponse>(new PutVacancyRequest(vacancy.Id, VacancyMapper.ToInnerDto(domainVacancy)));

            putResponse.EnsureSuccessStatusCode();
            return TypedResults.NoContent();
        }
        catch (ApiResponseException ex)
        {
            logger.LogError(ex, "Error while closing vacancy: {id}", request.VacancyId);
            return TypedResults.Problem(title: ex.Message, detail: ex.Error);
        }
    }
}