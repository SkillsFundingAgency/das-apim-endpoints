using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Extensions;
using SFA.DAS.Recruit.Api.Models.Requests;
using SFA.DAS.Recruit.Api.Models.Responses;
using SFA.DAS.Recruit.Api.Models.Vacancies;
using SFA.DAS.Recruit.Api.Models.Vacancies.Requests;
using SFA.DAS.Recruit.Application.Queries.GetNextVacancyReference;
using SFA.DAS.Recruit.Data.Models;
using SFA.DAS.Recruit.Domain.Vacancy;
using SFA.DAS.Recruit.GraphQL;
using SFA.DAS.Recruit.GraphQL.RecruitInner.Mappers;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using StrawberryShake;

using GetNextVacancyReferenceResponse = SFA.DAS.Recruit.Api.Models.Vacancies.Responses.GetNextVacancyReferenceResponse;
using VacancyStatus = SFA.DAS.Recruit.GraphQL.VacancyStatus;

namespace SFA.DAS.Recruit.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class VacanciesController(ILogger<VacanciesController> logger): ControllerBase
{
    [HttpGet, Route("vacancyReference")]
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
    
    [HttpGet, Route("employer/{accountId:int}/all")]
    public async Task<IResult> GetEmployerAllVacanciesList(
        [FromServices] IRecruitGqlClient recruitGqlClient,
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromRoute] long accountId,
        VacancyListFilterParams vacancyListFilterParams,
        SortParams<VacancySortColumn> sortParams,
        PageParams pageParams,
        CancellationToken cancellationToken = default)
    {
        var response = await recruitGqlClient.GetPagedVacanciesList.ExecuteAsync(
            vacancyListFilterParams.Build(accountId: accountId),
            sortParams.Build(),
            pageParams.Skip(),
            pageParams.Take(),
            cancellationToken
        );
        
        if (response.IsErrorResult())
        {
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var pageInfo = new PageInfo(pageParams.PageNumber!.Value, pageParams.PageSize!.Value, Convert.ToUInt32(response.Data?.PagedVacancies?.TotalCount ?? 0));
        var items = response.Data?.PagedVacancies?.Items ?? [];
        if (items is not { Count: > 0 })
        {
            return TypedResults.Ok(new PagedDataResponse<IEnumerable<VacancyListItem>>([], pageInfo));
        }
        
        var vacancyReferences = items
            .Where(x => x.VacancyReference is not null && x.Status is VacancyStatus.Live or VacancyStatus.Closed)
            .Select(x => x.VacancyReference!.Value);
        
        var statsResponse = await recruitApiClient.GetWithResponseCode<DataResponse<Dictionary<long, VacancyStatsItem>>>(new GetEmployerVacancyApplicationStatsRequest(accountId, vacancyReferences));
        statsResponse.EnsureSuccessStatusCode();
        
        var data = items.AssignStatsToVacancies(statsResponse.Body.Data ?? []);
        return TypedResults.Ok(new PagedDataResponse<IEnumerable<VacancyListItem>>(data, pageInfo));
    }
    
    [HttpGet, Route("provider/{ukprn:int}/all")]
    public async Task<IResult> GetProviderAllVacanciesList(
        [FromServices] IRecruitGqlClient recruitGqlClient,
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromRoute] int ukprn,
        VacancyListFilterParams vacancyListFilterParams,
        SortParams<VacancySortColumn> sortParams,
        PageParams pageParams,
        CancellationToken cancellationToken = default)
    {
        var response = await recruitGqlClient.GetPagedVacanciesList.ExecuteAsync(
            vacancyListFilterParams.Build(ukprn: ukprn),
            sortParams.Build(),
            pageParams.Skip(),
            pageParams.Take(),
            cancellationToken
        );
        
        if (response.IsErrorResult())
        {
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var pageInfo = new PageInfo(pageParams.PageNumber!.Value, pageParams.PageSize!.Value, Convert.ToUInt32(response.Data?.PagedVacancies?.TotalCount ?? 0));
        var items = response.Data?.PagedVacancies?.Items ?? [];
        if (items is not { Count: > 0 })
        {
            return TypedResults.Ok(new PagedDataResponse<IEnumerable<VacancyListItem>>([], pageInfo));
        }
        
        var vacancyReferences = items
            .Where(x => x.VacancyReference is not null && x.Status is VacancyStatus.Live or VacancyStatus.Closed)
            .Select(x => x.VacancyReference!.Value);
        
        var statsResponse = await recruitApiClient.GetWithResponseCode<DataResponse<Dictionary<long, VacancyStatsItem>>>(new GetProviderVacancyApplicationStatsRequest(ukprn, vacancyReferences));
        statsResponse.EnsureSuccessStatusCode();

        var data = items.AssignStatsToVacancies(statsResponse.Body.Data ?? []);
        return TypedResults.Ok(new PagedDataResponse<IEnumerable<VacancyListItem>>(data, pageInfo));
    }
    
    [HttpGet, Route("employer/{accountId:int}/draft")]
    public async Task<IResult> GetEmployerDraftVacanciesList(
        [FromServices] IRecruitGqlClient recruitGqlClient,
        [FromRoute] long accountId,
        VacancyListFilterParams vacancyListFilterParams,
        SortParams<VacancySortColumn> sortParams,
        PageParams pageParams,
        CancellationToken cancellationToken = default)
    {
        var response = await recruitGqlClient.GetPagedVacanciesList.ExecuteAsync(
            vacancyListFilterParams.Build(accountId: accountId, statuses: [VacancyStatus.Draft]),
            sortParams.Build(),
            pageParams.Skip(),
            pageParams.Take(),
            cancellationToken
        );
        
        if (response.IsErrorResult())
        {
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var pageInfo = new PageInfo(pageParams.PageNumber!.Value, pageParams.PageSize!.Value, Convert.ToUInt32(response.Data?.PagedVacancies?.TotalCount ?? 0));
        var items = response.Data?.PagedVacancies?.Items ?? [];
        var data = items is { Count: 0 } ? [] : items.Select(x => VacancyListItem.From(x, null));
        
        return TypedResults.Ok(new PagedDataResponse<IEnumerable<VacancyListItem>>(data, pageInfo));
    }

    [HttpGet, Route("provider/{ukprn:int}/{status}")]
    public async Task<IResult> GetProviderVacanciesListByStatus(
        [FromServices] IRecruitGqlClient recruitGqlClient,
        [FromRoute] int ukprn,
        [FromRoute] Domain.Vacancy.VacancyStatus status,
        VacancyListFilterParams vacancyListFilterParams,
        SortParams<VacancySortColumn> sortParams,
        PageParams pageParams,
        CancellationToken cancellationToken = default)
    {
        if (!GqlVacancyStatusMapper.TryMapToGqlStatus(status, out var gqlStatus))
            return TypedResults.BadRequest(new { message = $"Unsupported status '{status}'." });

        if (!Enum.IsDefined(typeof(VacancyStatus), gqlStatus) ||
            (gqlStatus != VacancyStatus.Draft && gqlStatus != VacancyStatus.Review))
        {
            return TypedResults.BadRequest(new { message = "Status must be 'draft' or 'review'." });
        }

        var response = await recruitGqlClient.GetPagedVacanciesList.ExecuteAsync(
            vacancyListFilterParams.Build(ukprn: ukprn, statuses: [gqlStatus]),
            sortParams.Build(),
            pageParams.Skip(),
            pageParams.Take(),
            cancellationToken
        );

        if (response.IsErrorResult())
            return TypedResults.Problem(response.ToProblemDetails());

        var total = Convert.ToUInt32(response.Data?.PagedVacancies?.TotalCount ?? 0);
        var pageInfo = new PageInfo(pageParams.PageNumber!.Value, pageParams.PageSize!.Value, total);

        var items = response.Data?.PagedVacancies?.Items ?? [];
        var data = items.Count == 0 ? [] : items.Select(x => VacancyListItem.From(x, null));

        return TypedResults.Ok(new PagedDataResponse<IEnumerable<VacancyListItem>>(data, pageInfo));
    }
}