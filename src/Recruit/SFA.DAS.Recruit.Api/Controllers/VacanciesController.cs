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
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.VacancyAnalytics;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses.VacancyAnalytics;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using StrawberryShake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
    
    [HttpGet, Route("employer/{accountId:int}/{status:regex(^(draft|submitted)$)}")]
    public async Task<IResult> GetEmployerVacanciesListByStatus(
        [FromServices] IRecruitGqlClient recruitGqlClient,
        [FromRoute] long accountId,
        [FromRoute] Domain.Vacancy.VacancyStatus status,
        VacancyListFilterParams vacancyListFilterParams,
        SortParams<VacancySortColumn> sortParams,
        PageParams pageParams,
        CancellationToken cancellationToken = default)
    {
        if (!GqlTypeExtensions.TryMapToGqlStatus(status, out var gqlStatus))
            return TypedResults.BadRequest(new { message = $"Unsupported status '{status}'." });

        var response = await recruitGqlClient.GetPagedVacanciesList.ExecuteAsync(
            vacancyListFilterParams.Build(accountId: accountId, statuses: [gqlStatus]),
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

    [HttpGet, Route("provider/{ukprn:int}/{status:regex(^(draft|review|submitted)$)}")]
    public async Task<IResult> GetProviderVacanciesListByStatus(
        [FromServices] IRecruitGqlClient recruitGqlClient,
        [FromRoute] int ukprn,
        [FromRoute] Domain.Vacancy.VacancyStatus status,
        VacancyListFilterParams vacancyListFilterParams,
        SortParams<VacancySortColumn> sortParams,
        PageParams pageParams,
        CancellationToken cancellationToken = default)
    {
        if (!GqlTypeExtensions.TryMapToGqlStatus(status, out var gqlStatus))
            return TypedResults.BadRequest(new { message = $"Unsupported status '{status}'." });

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