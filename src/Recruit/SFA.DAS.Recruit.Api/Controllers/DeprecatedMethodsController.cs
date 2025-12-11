using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models.Vacancies.Responses;
using SFA.DAS.Recruit.Domain.Vacancy;
using SFA.DAS.Recruit.GraphQL;
using SFA.DAS.Recruit.GraphQL.RecruitInner.Mappers;
using StrawberryShake;
using VacancyStatus = SFA.DAS.Recruit.Domain.Vacancy.VacancyStatus;

namespace SFA.DAS.Recruit.Api.Controllers;

[ApiController, Route("deprecated/vacancies/")]
public class DeprecatedMethodsController(ILogger<DeprecatedMethodsController> logger): ControllerBase
{
    /*
     * Transitional controller to put methods that used to exist in the Recruit project and
     * haven't yet been replaced with the middle-tier controller/action architectural approach.
     * Routes are intentionally completely non-rest like.
     */
    
    [HttpGet, Route("getVacanciesByStatusAndClosingDate")]
    [ProducesResponseType(typeof(IEnumerable<VacancyIdentity>), StatusCodes.Status200OK)]
    public async Task<IResult> GetVacanciesByStatusAndClosingDate(
        [FromQuery, Required] VacancyStatus status,
        [FromQuery, Required] DateTime closingDate,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient.GetVacanciesByStatusAndClosingDate.ExecuteAsync(status.ToQueryType(), closingDate, cancellationToken);
        if (!response.IsSuccessResult())
        {
            logger.LogError(response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var results = response.Data!.Vacancies.Select(x => new VacancyIdentity
        {
            Id = x.Id,
            VacancyReference = x.VacancyReference,
            ClosingDate = x.ClosingDate?.UtcDateTime,
            Status = x.Status.FromQueryType(),
        });
        return TypedResults.Ok(results);
    }
    
    [HttpPost, Route("findClosedVacancies")]
    [ProducesResponseType(typeof(Vacancy), StatusCodes.Status200OK)]
    public async Task<IResult> FindClosedVacancies(
        [FromBody, Required] IEnumerable<long> vacancyReferences,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient.FindClosedVacancies.ExecuteAsync(vacancyReferences.Cast<long?>().ToList(), cancellationToken);
        if (!response.IsSuccessResult())
        {
            logger.LogError(response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }
        
        var vacancies = response.Data!.Vacancies.Select(GqlVacancyMapper.From);
        return TypedResults.Ok(vacancies);
    }
    
    [HttpGet, Route("getProviderVacancies")]
    [ProducesResponseType(typeof(Vacancy), StatusCodes.Status200OK)]
    public async Task<IResult> GetProviderVacancies(
        [FromQuery, Required] int ukprn,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient.GetProviderVacancies.ExecuteAsync(ukprn, cancellationToken);
        if (!response.IsSuccessResult())
        {
            logger.LogError(response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }
        
        var results = response.Data!.Vacancies.Select(x => new GetProviderVacanciesItem
        {
            Id = x.Id,
            VacancyReference = x.VacancyReference,
            Status = x.Status.FromQueryType(),
            OwnerType = x.OwnerType.FromQueryType()!.Value,
            AccountId = x.AccountId
        });
        
        return TypedResults.Ok(results);
    }
    
    [HttpGet, Route("getProviderOwnedVacanciesForLegalEntity")]
    [ProducesResponseType(typeof(Vacancy), StatusCodes.Status200OK)]
    public async Task<IResult> GetProviderOwnedVacanciesForLegalEntity(
        [FromQuery, Required] int ukprn,
        [FromQuery, Required] long legalEntityId, 
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient.GetProviderOwnedVacanciesForLegalEntity.ExecuteAsync(ukprn, legalEntityId, cancellationToken);
        if (!response.IsSuccessResult())
        {
            logger.LogError(response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }
        
        var vacancies = response.Data!.Vacancies.Select(GqlVacancyMapper.From);
        return TypedResults.Ok(vacancies);
    }
    
    [HttpGet, Route("getProviderOwnedVacanciesInReviewForLegalEntity")]
    [ProducesResponseType(typeof(Vacancy), StatusCodes.Status200OK)]
    public async Task<IResult> GetProviderOwnedVacanciesInReview(
        [FromQuery, Required] int ukprn,
        [FromQuery, Required] long legalEntityId,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient.GetProviderOwnedVacanciesInReview.ExecuteAsync(ukprn, legalEntityId, cancellationToken);
        if (!response.IsSuccessResult())
        {
            logger.LogError(response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }
        
        var vacancies = response.Data!.Vacancies.Select(GqlVacancyMapper.From);
        return TypedResults.Ok(vacancies);
    }
    
    [HttpGet, Route("getProviderOwnedVacanciesForEmployerWithoutAccountLegalEntityId")]
    [ProducesResponseType(typeof(Vacancy), StatusCodes.Status200OK)]
    public async Task<IResult> GetProviderOwnedVacanciesForEmployerWithoutAccountLegalEntityId(
        [FromQuery, Required] int ukprn,
        [FromQuery, Required] long accountId,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient.GetProviderOwnedVacanciesForEmployerWithoutAccountLegalEntityId.ExecuteAsync(ukprn, accountId, cancellationToken);
        if (!response.IsSuccessResult())
        {
            logger.LogError(response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }
        
        var vacancies = response.Data!.Vacancies.Select(GqlVacancyMapper.From);
        return TypedResults.Ok(vacancies);
    }
    
    [HttpGet, Route("getDraftVacanciesCreatedBefore")]
    [ProducesResponseType(typeof(IEnumerable<VacancyIdentity>), StatusCodes.Status200OK)]
    public async Task<IResult> GetDraftVacanciesCreatedBefore(
        [FromQuery, Required] DateTime createdDate,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient.GetDraftVacanciesCreatedBefore.ExecuteAsync(createdDate, cancellationToken);
        if (!response.IsSuccessResult())
        {
            logger.LogError(response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var results = response.Data!.Vacancies.Select(x => new VacancyIdentity
        {
            Id = x.Id,
            VacancyReference = x.VacancyReference,
            ClosingDate = x.ClosingDate?.UtcDateTime,
            Status = x.Status.FromQueryType(),
        });
        return TypedResults.Ok(results);
    }
    
    [HttpGet, Route("getReferredVacanciesSubmittedBefore")]
    [ProducesResponseType(typeof(IEnumerable<VacancyIdentity>), StatusCodes.Status200OK)]
    public async Task<IResult> GetReferredVacanciesSubmittedBefore(
        [FromQuery, Required] DateTime createdDate,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient.GetReferredVacanciesSubmittedBefore.ExecuteAsync(createdDate, cancellationToken);
        if (!response.IsSuccessResult())
        {
            logger.LogError(response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var results = response.Data!.Vacancies.Select(x => new VacancyIdentity
        {
            Id = x.Id,
            VacancyReference = x.VacancyReference,
            ClosingDate = x.ClosingDate?.UtcDateTime,
            Status = x.Status.FromQueryType(),
        });
        return TypedResults.Ok(results);
    }
}