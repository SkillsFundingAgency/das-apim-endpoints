using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.GraphQL;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.Data.Models;
using SFA.DAS.RecruitQa.Domain;
using SFA.DAS.RecruitQa.GraphQL.RecruitInner.Mappers;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using StrawberryShake;
using Wage = SFA.DAS.Recruit.Contracts.ApiResponses.Wage;

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
    
    [HttpPost, Route("update-from-qa/{id:Guid}")]
    public async Task<IResult> UpdateVacancyFromQaEdit([FromRoute] Guid id,
        [FromServices] IRecruitApiClient<RecruitAiApiConfiguration> recruitApiClient,
        [FromBody] UpdateVacancyRequest request,
        CancellationToken cancellationToken)
    {
        await recruitApiClient.Patch(new Recruit.Contracts.ApiRequests.PatchVacanciesByVacancyIdApiRequest
        {
            VacancyId = id,
            Data = new Recruit.Contracts.ApiResponses.Vacancy
            {
                Status = Enum.Parse<Recruit.Contracts.ApiResponses.VacancyStatus>(request.Status),
                OutcomeDescription = request.OutcomeDescription,
                TrainingDescription = request.TrainingDescription,
                AdditionalTrainingDescription = request.AdditionalTrainingDescription,
                ShortDescription = request.ShortDescription,
                Description = request.Description,
                Wage = new Wage
                {
                    CompanyBenefitsInformation =  request.CompanyBenefitsInformation,
                    WorkingWeekDescription =  request.WorkingWeekDescription
                },
                ThingsToConsider =  request.ThingsToConsider,
                ApplicationInstructions =  request.ApplicationInstructions,
                LastUpdatedDate = DateTime.UtcNow,
                EmployerLocationInformation =  request.EmployerLocationInformation,
            }
        });
        
        return TypedResults.Ok();
    }
    
    [HttpPost, Route("close/{id:Guid}")]
    public async Task<IResult> CloseVacancyFromQa([FromRoute] Guid id,
        [FromServices] IRecruitApiClient<RecruitAiApiConfiguration> recruitApiClient,
        [FromBody] CloseVacancyRequest request,
        CancellationToken cancellationToken)
    {
        await recruitApiClient.Patch(new Recruit.Contracts.ApiRequests.PatchVacanciesByVacancyIdApiRequest
        {
            VacancyId = id,
            Data = new Recruit.Contracts.ApiResponses.Vacancy
            {
                Status = Recruit.Contracts.ApiResponses.VacancyStatus.Closed,
                LastUpdatedDate = DateTime.UtcNow,
                ClosureReason =  Enum.Parse<Recruit.Contracts.ApiResponses.ClosureReason>(request.ClosureReason)
            }
        });
        
        return TypedResults.Ok();
    }
    
    [HttpPost, Route("publish/{id:Guid}")]
    public async Task<IResult> PublishVacancyFromQa([FromRoute] Guid id,
        [FromServices] IRecruitApiClient<RecruitAiApiConfiguration> recruitApiClient,
        CancellationToken cancellationToken)
    {
        await recruitApiClient.Patch(new Recruit.Contracts.ApiRequests.PatchVacanciesByVacancyIdApiRequest
        {
            VacancyId = id,
            Data = new Recruit.Contracts.ApiResponses.Vacancy
            {
                Status = Recruit.Contracts.ApiResponses.VacancyStatus.Live,
                LastUpdatedDate = DateTime.UtcNow
            }
        });
        
        return TypedResults.Ok();
    }
}