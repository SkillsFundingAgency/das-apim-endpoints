using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Api.Models.Vacancies.Responses;
using SFA.DAS.Recruit.Data.Models;
using SFA.DAS.Recruit.Domain.Vacancy;
using SFA.DAS.Recruit.GraphQL;
using SFA.DAS.Recruit.GraphQL.RecruitInner.Mappers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
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
    
    [HttpGet, Route("getVacanciesToClose")]
    [ProducesResponseType(typeof(DataResponse<IEnumerable<VacancyIdentifier>>), StatusCodes.Status200OK)]
    public async Task<IResult> GetVacanciesToClose(
        [FromQuery, Required] DateTime pointInTime,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient.GetVacanciesToClose.ExecuteAsync(pointInTime, cancellationToken);
        if (!response.IsSuccessResult())
        {
            logger.LogError(response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var data = response.Data!.Vacancies.Select(x => new VacancyIdentifier(x.Id, x.VacancyReference, VacancyStatus.Live, x.ClosingDate!.Value.UtcDateTime)); 
        return TypedResults.Ok(new DataResponse<IEnumerable<VacancyIdentifier>>(data));
    }
    
    [HttpPost, Route("findClosedVacancies")]
    [ProducesResponseType(typeof(DataResponse<IEnumerable<Vacancy>>), StatusCodes.Status200OK)]
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

        var data = response.Data!.Vacancies.Select(GqlVacancyMapper.From);
        return TypedResults.Ok(new DataResponse<IEnumerable<Vacancy>>(data));
    }
    
    [HttpGet, Route("getProviderVacancies")]
    [ProducesResponseType(typeof(DataResponse<IEnumerable<GetProviderVacanciesItem>>), StatusCodes.Status200OK)]
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

        var data = response.Data!.Vacancies.Select(x => new GetProviderVacanciesItem
        {
            Id = x.Id,
            VacancyReference = x.VacancyReference,
            Status = x.Status.FromQueryType(),
            OwnerType = x.OwnerType.FromQueryType()!.Value,
            AccountId = x.AccountId
        });
        return TypedResults.Ok(new DataResponse<IEnumerable<GetProviderVacanciesItem>>(data));
    }
    
    [HttpGet, Route("getProviderOwnedVacanciesForLegalEntity")]
    [ProducesResponseType(typeof(DataResponse<IEnumerable<Vacancy>>), StatusCodes.Status200OK)]
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

        var data = response.Data!.Vacancies.Select(GqlVacancyMapper.From);
        return TypedResults.Ok(new DataResponse<IEnumerable<Vacancy>>(data));
    }
    
    [HttpGet, Route("getProviderOwnedVacanciesInReviewForLegalEntity")]
    [ProducesResponseType(typeof(DataResponse<IEnumerable<Vacancy>>), StatusCodes.Status200OK)]
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

        var data = response.Data!.Vacancies.Select(GqlVacancyMapper.From);
        return TypedResults.Ok(new DataResponse<IEnumerable<Vacancy>>(data));
    }
    
    [HttpGet, Route("getProviderOwnedVacanciesForEmployerWithoutAccountLegalEntityId")]
    [ProducesResponseType(typeof(DataResponse<IEnumerable<Vacancy>>), StatusCodes.Status200OK)]
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

        var data = response.Data!.Vacancies.Select(GqlVacancyMapper.From);
        return TypedResults.Ok(new DataResponse<IEnumerable<Vacancy>>(data));
    }
    
    [HttpGet, Route("getDraftVacanciesCreatedBefore")]
    [ProducesResponseType(typeof(DataResponse<IEnumerable<VacancyIdentifier>>), StatusCodes.Status200OK)]
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

        var data = response.Data!.Vacancies.Select(x => new VacancyIdentifier(
            x.Id,
            x.VacancyReference,
            x.Status.FromQueryType(),
            x.ClosingDate?.UtcDateTime
        ));
        return TypedResults.Ok(new DataResponse<IEnumerable<VacancyIdentifier>>(data));
    }
    
    [HttpGet, Route("getReferredVacanciesSubmittedBefore")]
    [ProducesResponseType(typeof(DataResponse<IEnumerable<VacancyIdentifier>>), StatusCodes.Status200OK)]
    public async Task<IResult> GetReferredVacanciesSubmittedBefore(
        [FromQuery, Required] DateTime submittedDate,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient.GetReferredVacanciesSubmittedBefore.ExecuteAsync(submittedDate, cancellationToken);
        if (!response.IsSuccessResult())
        {
            logger.LogError(response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var data = response.Data!.Vacancies.Select(x => new VacancyIdentifier(
            x.Id,
            x.VacancyReference,
            x.Status.FromQueryType(),
            x.ClosingDate?.UtcDateTime
        ));
        return TypedResults.Ok(new DataResponse<IEnumerable<VacancyIdentifier>>(data));
    }
    
    [HttpGet, Route("getVacancyCountForUser")]
    [ProducesResponseType(typeof(DataResponse<int>), StatusCodes.Status200OK)]
    public async Task<IResult> GetVacancyCountForUser(
        [FromQuery, Required] Guid userId,
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.Get<DataResponse<int>>(new GetRequest($"/api/vacancies/count/user/{userId}"));
        return TypedResults.Ok(response);
    }
}