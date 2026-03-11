using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Api.Models.Vacancies;
using SFA.DAS.Recruit.Api.Models.Vacancies.Requests;
using SFA.DAS.Recruit.Application.Queries.GetNextVacancyReference;
using SFA.DAS.Recruit.Domain.Vacancy;
using SFA.DAS.Recruit.GraphQL;
using SFA.DAS.Recruit.GraphQL.RecruitInner.Mappers;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.VacancyAnalytics;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses.VacancyAnalytics;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GetNextVacancyReferenceResponse = SFA.DAS.Recruit.Api.Models.Vacancies.Responses.GetNextVacancyReferenceResponse;

namespace SFA.DAS.Recruit.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class VacanciesController(ILogger<VacanciesController> logger): ControllerBase
{
    [HttpGet, Route("vacancyreference")]
    public async Task<IActionResult> GetNextVacancyReference([FromServices] IMediator mediator)
    {
        var result = await mediator.Send(new GetNextVacancyReferenceQuery());
        return Ok(new GetNextVacancyReferenceResponse(result.Value));
    }

    // TODO: Semi proxy for the inner api endpoint - this should go once we have migrated vacancies over to SQL
    [HttpPost, Route("{vacancyId:guid}")]
    public async Task<IActionResult> PostOne(
        [FromRoute] Guid vacancyId,
        [FromBody] PostVacancyRequest vacancy,
        [FromServices] VacancyMapper vacancyMapper,
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    {
        var response = await recruitApiClient.PutWithResponseCode<PutVacancyResponse>(new PutVacancyRequest(vacancyId, vacancyMapper.ToInnerDto(vacancy)));
        try
        {
            response.EnsureSuccessStatusCode();
            return Ok(vacancyMapper.ToOuterDto(response.Body));
        }
        catch (ApiResponseException ex)
        {
            logger.LogError(ex, "Error updating vacancy");
            return Problem(title: ex.Message, detail: ex.Error);
        }
    }

    [HttpGet, Route("{vacancyId:guid}")]
    [ProducesResponseType(typeof(DataResponse<Vacancy>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetOneById([FromRoute] Guid vacancyId,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var result = await recruitGqlClient.GetVacancyById.ExecuteAsync(vacancyId, cancellationToken);
        return result is { Data.Vacancies.Count: 1 }
            ? TypedResults.Ok(new DataResponse<Vacancy>(GqlVacancyMapper.From(result.Data.Vacancies[0])))
            : TypedResults.NotFound();
    }
    
    [HttpGet, Route("by/ref/{vacancyReference:long}")]
    [ProducesResponseType(typeof(DataResponse<Vacancy>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetOneByReference([FromRoute] long vacancyReference,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var result = await recruitGqlClient.GetVacancyByReference.ExecuteAsync(vacancyReference, cancellationToken);
        return result is { Data.Vacancies.Count: 1 }
            ? TypedResults.Ok(new DataResponse<Vacancy>(GqlVacancyMapper.From(result.Data.Vacancies[0])))
            : TypedResults.NotFound();
    }

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
}