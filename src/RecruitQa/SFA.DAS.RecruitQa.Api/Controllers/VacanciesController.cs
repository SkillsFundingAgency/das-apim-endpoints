using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.GraphQL;
using SFA.DAS.RecruitQa.Data.Models;
using SFA.DAS.RecruitQa.Domain;
using SFA.DAS.RecruitQa.GraphQL.RecruitInner.Mappers;
using StrawberryShake;

namespace SFA.DAS.RecruitQa.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class VacanciesController: ControllerBase
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
}