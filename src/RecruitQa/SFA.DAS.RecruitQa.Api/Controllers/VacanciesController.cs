using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Recruit.GraphQL;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.Data.Models;
using SFA.DAS.RecruitQa.Domain;
using SFA.DAS.RecruitQa.GraphQL.RecruitInner.Mappers;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using StrawberryShake;

namespace SFA.DAS.RecruitQa.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class VacanciesController(ILogger<VacanciesController> logger): ControllerBase
{
    [HttpGet, Route("{vacancyId:guid}")]
    [ProducesResponseType(typeof(DataResponse<Vacancy>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetOneById([FromRoute] Guid vacancyId,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient.GetVacancyById.ExecuteAsync(vacancyId, cancellationToken);
        
        if (response.IsErrorResult())
        {
            return TypedResults.Problem(response.ToProblemDetails());
        }
        
        return response is { Data.Vacancies.Count: 1 }
            ? TypedResults.Ok(new DataResponse<Vacancy>(GqlVacancyMapper.From(response.Data.Vacancies[0])))
            : TypedResults.NotFound();
    }

    
    [HttpGet, Route("by/ref/{vacancyReference:long}")]
    [ProducesResponseType(typeof(DataResponse<Vacancy>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetOneByReference([FromRoute] long vacancyReference,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient.GetVacancyByReference.ExecuteAsync(vacancyReference, cancellationToken);
        
        if (response.IsErrorResult())
        {
            return TypedResults.Problem(response.ToProblemDetails());
        }
        
        return response is { Data.Vacancies.Count: 1 }
            ? TypedResults.Ok(new DataResponse<Vacancy>(GqlVacancyMapper.From(response.Data.Vacancies[0])))
            : TypedResults.NotFound();
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
}