using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Shared.Infrastructure;
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
    
    [HttpPost, Route("update-from-qa/{id:Guid}")]
    public async Task<IResult> UpdateVacancyFromQaEdit([FromRoute] Guid id,
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromBody] UpdateVacancyRequest request,
        CancellationToken cancellationToken)
    {
        var data = new JsonPatchDocument<Recruit.Contracts.ApiResponses.Vacancy>();
        data.Replace(x => x.Status, Enum.Parse<Recruit.Contracts.ApiResponses.VacancyStatus>(request.Status));
        data.Replace(x => x.LastUpdatedDate, DateTime.UtcNow);
        if (!string.IsNullOrEmpty(request.CompanyBenefitsInformation))
        {
            data.Replace(x => x.Wage.CompanyBenefitsInformation, request.CompanyBenefitsInformation);    
        }
        if (!string.IsNullOrEmpty(request.WorkingWeekDescription))
        {
            data.Replace(x => x.Wage.WorkingWeekDescription, request.WorkingWeekDescription);    
        }
        if (!string.IsNullOrEmpty(request.OutcomeDescription))
        {
            data.Replace(x => x.OutcomeDescription, request.OutcomeDescription);    
        }
        if (!string.IsNullOrEmpty(request.TrainingDescription))
        {
            data.Replace(x => x.TrainingDescription, request.TrainingDescription);    
        }
        if (!string.IsNullOrEmpty(request.AdditionalTrainingDescription))
        {
            data.Replace(x => x.AdditionalTrainingDescription, request.AdditionalTrainingDescription);    
        }
        if (!string.IsNullOrEmpty(request.ShortDescription))
        {
            data.Replace(x => x.ShortDescription, request.ShortDescription);    
        }
        if (!string.IsNullOrEmpty(request.Description))
        {
            data.Replace(x => x.Description, request.Description);    
        }
        if (!string.IsNullOrEmpty(request.ThingsToConsider))
        {
            data.Replace(x => x.ThingsToConsider, request.ThingsToConsider);    
        }
        if (!string.IsNullOrEmpty(request.ApplicationInstructions))
        {
            data.Replace(x => x.ApplicationInstructions, request.ApplicationInstructions);    
        }
        if (!string.IsNullOrEmpty(request.EmployerLocationInformation))
        {
            data.Replace(x => x.EmployerLocationInformation, request.EmployerLocationInformation);    
        }

        await recruitApiClient.PatchWithResponseCode(new Recruit.Contracts.ApiRequests.PatchVacanciesByVacancyIdApiRequest
        {
            VacancyId = id,
            Data = data
        });
        
        return TypedResults.Ok();
    }
    
    [HttpPost, Route("close/{id:Guid}")]
    public async Task<IResult> CloseVacancyFromQa([FromRoute] Guid id,
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromBody] CloseVacancyRequest request,
        CancellationToken cancellationToken)
    {
        var data = new JsonPatchDocument<Recruit.Contracts.ApiResponses.Vacancy>();
        data.Replace(x => x.Status, Recruit.Contracts.ApiResponses.VacancyStatus.Closed);
        data.Replace(x => x.LastUpdatedDate, DateTime.UtcNow);
        data.Replace(x => x.ClosureReason, Enum.Parse<Recruit.Contracts.ApiResponses.ClosureReason>(request.ClosureReason));
        
        await recruitApiClient.PatchWithResponseCode(new Recruit.Contracts.ApiRequests.PatchVacanciesByVacancyIdApiRequest
        {
            VacancyId = id,
            Data = data
        });
        
        return TypedResults.Ok();
    }
    
    [HttpPost, Route("publish/{id:Guid}")]
    public async Task<IResult> PublishVacancyFromQa([FromRoute] Guid id,
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        CancellationToken cancellationToken)
    {
        var data = new JsonPatchDocument<Recruit.Contracts.ApiResponses.Vacancy>();
        data.Replace(x => x.Status, Recruit.Contracts.ApiResponses.VacancyStatus.Live);
        data.Replace(x => x.LastUpdatedDate, DateTime.UtcNow);
        
        await recruitApiClient.PatchWithResponseCode(new Recruit.Contracts.ApiRequests.PatchVacanciesByVacancyIdApiRequest
        {
            VacancyId = id,
            Data = data
        });
        
        return TypedResults.Ok();
    }
}