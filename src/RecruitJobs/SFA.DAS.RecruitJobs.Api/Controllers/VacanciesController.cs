using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitJobs.Api.Models;
using SFA.DAS.RecruitJobs.Api.Models.Requests;
using SFA.DAS.RecruitJobs.Api.Models.Vacancies.Responses;
using SFA.DAS.RecruitJobs.GraphQL;
using SFA.DAS.RecruitJobs.GraphQL.RecruitInner.Mappers;
using StrawberryShake;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using SFA.DAS.RecruitJobs.Api.Models.Mappers;
using VacancyStatus = SFA.DAS.Recruit.Contracts.ApiResponses.VacancyStatus;
using ValidationProblemDetails = Microsoft.AspNetCore.Mvc.ValidationProblemDetails;

namespace SFA.DAS.RecruitJobs.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class VacanciesController(ILogger<VacanciesController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{id:guid}")]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(DataResponse<Vacancy>), StatusCodes.Status200OK)]
    public async Task<IResult> GetOneById(
        [FromServices] IRecruitGqlClient recruitGqlClient,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient.GetVacancyById.ExecuteAsync(id, cancellationToken);
        if (!response.IsSuccessResult())
        {
            logger.LogError("Error fetching vacancy '{VacancyId}':\r\n {Errors}", id, response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }
        
        if (response is not { Data.Vacancies.Count: > 0 })
        {
            return TypedResults.NotFound();
        }

        var vacancy = response.Data.Vacancies[0];
        var domainVacancy = GqlVacancyMapper.ToDomain(vacancy);

        return TypedResults.Ok(new DataResponse<Vacancy>(domainVacancy));
    }

    [HttpGet]
    [Route("byRef/{vacancyReference:long}")]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(DataResponse<Vacancy>), StatusCodes.Status200OK)]
    public async Task<IResult> GetOneByVacancyReference(
        [FromServices] IRecruitGqlClient recruitGqlClient,
        [FromRoute] long vacancyReference,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient.GetVacancyByReference.ExecuteAsync(vacancyReference, cancellationToken);
        if (!response.IsSuccessResult())
        {
            logger.LogError("Error fetching vacancy '{VacancyReference}':\r\n {Errors}", vacancyReference, response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }

        if (response is not { Data.Vacancies.Count: > 0 })
        {
            return TypedResults.NotFound();
        }

        var vacancy = response.Data.Vacancies[0];
        var domainVacancy = GqlVacancyMapper.ToDomain(vacancy);

        return TypedResults.Ok(new DataResponse<Vacancy>(domainVacancy));
    }

    [HttpGet]
    [Route("{vacancyReference:long}/analytics")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(VacancyAnalyticsResponse), StatusCodes.Status200OK)]
    public async Task<IResult> GetOne(
        [FromServices] Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration> recruitApiClient,
        [FromRoute] long vacancyReference,
        CancellationToken token = default)
    {
        try
        {
            logger.LogInformation("Recruit API: Received request to get vacancy analytics for vacancy reference: {VacancyReference}", vacancyReference);

            var result = await recruitApiClient.Get<VacancyAnalyticsResponse>(new GetVacancyanalyticsByVacancyReferenceApiRequest(vacancyReference));

            var response = result ?? new VacancyAnalyticsResponse
            {
                VacancyReference = vacancyReference,
                Analytics = []
            };

            return TypedResults.Ok(response);
        }
        catch (Exception e)
        {
            logger.LogError(e,
                "Unable to get vacancy analytics : An error occurred");
            return Results.Problem(statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPut]
    [Route("{vacancyReference:long}/analytics")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IResult> PutOne(
        [FromServices] Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration> recruitApiClient,
        [FromRoute] long vacancyReference, 
        [FromBody] PutVacancyAnalyticsRequest request,
        CancellationToken token = default)
    {
        try
        {
            logger.LogInformation("Recruit API: Received request to create vacancy analytics for vacancy reference: {VacancyReference}", vacancyReference);

            await recruitApiClient.Put(new PutVacancyanalyticsByVacancyReferenceApiRequest
            {
                VacancyReference = vacancyReference,
                Data = request
            });

            return TypedResults.Created();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to create vacancy analytics : An error occurred");
            return Results.Problem(statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet, Route("stale/live")]
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

    [HttpGet, Route("stale/draft")]
    [ProducesResponseType(typeof(DataResponse<IEnumerable<StaleVacancyIdentifier>>), StatusCodes.Status200OK)]
    public async Task<IResult> GetDraftVacanciesToClose(
        [FromQuery, Required] DateTime pointInTime,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient
            .GetDraftVacanciesCreatedBefore
            .ExecuteAsync(pointInTime, cancellationToken);

        if (!response.IsSuccessResult())
        {
            logger.LogError("An error occured at GetDraftVacanciesToClose: {Errors}", response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var data = response
            .Data!
            .Vacancies
            .Select(x =>
                new StaleVacancyIdentifier(x.Id, x.VacancyReference, VacancyStatus.Draft, x.CreatedDate!.Value.UtcDateTime));
        return TypedResults.Ok(new DataResponse<IEnumerable<StaleVacancyIdentifier>>(data));
    }

    [HttpGet, Route("stale/employer/reviewed")]
    [ProducesResponseType(typeof(DataResponse<IEnumerable<StaleVacancyIdentifier>>), StatusCodes.Status200OK)]
    public async Task<IResult> GetEmployerReviewedVacanciesToClose(
        [FromQuery, Required] DateTime pointInTime,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient
            .GetEmployerReviewedVacanciesCreatedBefore
            .ExecuteAsync(pointInTime, cancellationToken);

        if (!response.IsSuccessResult())
        {
            logger.LogError("An error occured at GetEmployerReviewedVacanciesToClose: {Errors}", response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var data = response
            .Data!
            .Vacancies
            .Select(x =>
                new StaleVacancyIdentifier(x.Id, x.VacancyReference, VacancyStatus.Review, x.CreatedDate!.Value.UtcDateTime));
        return TypedResults.Ok(new DataResponse<IEnumerable<StaleVacancyIdentifier>>(data));
    }

    [HttpGet, Route("stale/employer/rejected")]
    [ProducesResponseType(typeof(DataResponse<IEnumerable<StaleVacancyIdentifier>>), StatusCodes.Status200OK)]
    public async Task<IResult> GetEmployerRejectedVacanciesToClose(
        [FromQuery, Required] DateTime pointInTime,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient
            .GetEmployerRejectedVacanciesCreatedBefore
            .ExecuteAsync(pointInTime, cancellationToken);

        if (!response.IsSuccessResult())
        {
            logger.LogError("An error occured at GetEmployerRejectedVacanciesToClose: {Errors}", response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var data = response
            .Data!
            .Vacancies
            .Select(x =>
                new StaleVacancyIdentifier(x.Id, x.VacancyReference, VacancyStatus.Rejected, x.CreatedDate!.Value.UtcDateTime));
        return TypedResults.Ok(new DataResponse<IEnumerable<StaleVacancyIdentifier>>(data));
    }

    [HttpGet, Route("stale/qa/rejected")]
    [ProducesResponseType(typeof(DataResponse<IEnumerable<StaleVacancyIdentifier>>), StatusCodes.Status200OK)]
    public async Task<IResult> GetQaRejectedVacanciesToClose(
        [FromQuery, Required] DateTime pointInTime,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient
            .GetQaRejectedVacanciesCreatedBefore
            .ExecuteAsync(pointInTime, cancellationToken);

        if (!response.IsSuccessResult())
        {
            logger.LogError("An error occured at GetQaRejectedVacanciesToClose: {Errors}", response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var data = response
            .Data!
            .Vacancies
            .Select(x =>
                new StaleVacancyIdentifier(x.Id, x.VacancyReference, VacancyStatus.Referred, x.CreatedDate!.Value.UtcDateTime));
        return TypedResults.Ok(new DataResponse<IEnumerable<StaleVacancyIdentifier>>(data));
    }

    [HttpPost, Route("delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> DeleteOne(
        [FromServices] Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration> recruitApiClient,
        [FromBody, Required] Guid id)
    {
        var results = await recruitApiClient.DeleteWithResponseCode<NullResponse>(new DeleteVacanciesByVacancyIdApiRequest(id));
        return Results.StatusCode((int)results.StatusCode);
    }

    [HttpPost, Route("{vacancyReference:long}/close")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IResult> CloseVacancy(
        [FromRoute] long vacancyReference,
        [FromBody] CloseVacancyRequest request,
        [FromServices] VacancyMapper vacancyMapper,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        [FromServices] Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration> recruitApiClient,
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
            if(vacancy is null || vacancy.VacancyReference != vacancyReference)
            {
                logger.LogWarning("Vacancy with id {VacancyId} not found at CloseVacancy", request.VacancyId);
                return TypedResults.NotFound();
            }

            var domainVacancy = GqlVacancyMapper.ToDomain(vacancy);
            domainVacancy.ClosureReason = request.ClosureReason;
            domainVacancy.Status = VacancyStatus.Closed;
            domainVacancy.ClosedDate = DateTime.UtcNow;

            var putResponse = await recruitApiClient.PutWithResponseCode<PutVacancyRequest, SFA.DAS.Recruit.Contracts.ApiResponses.Vacancy>(
                new PutVacanciesByVacancyIdApiRequest
                {
                    VacancyId = request.VacancyId,
                    RuleSet = VacancyRuleSet.All,
                    ValidateOnly = false,
                    Data = vacancyMapper.ToInnerDto(domainVacancy)
                });

            putResponse.EnsureSuccessStatusCode();
            return TypedResults.NoContent();
        }
        catch (ApiResponseException ex)
        {
            logger.LogError(ex, "Error while closing vacancy: {id}", request.VacancyId);
            return TypedResults.Problem(title: ex.Message, detail: ex.Error);
        }
    }
    
    [HttpPost, Route("{vacancyId:guid}/approve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> ApproveVacancy(
        [FromServices] IMessageSession messageSession,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        [FromServices] Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration> recruitApiClient,
        [FromRoute] Guid vacancyId,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient.GetVacancyById.ExecuteAsync(vacancyId, cancellationToken);
        if (!response.IsSuccessResult())
        {
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var vacancy = response.Data!.Vacancies.FirstOrDefault();
        if (vacancy is { TransferInfo: not null, Status: not GraphQL.VacancyStatus.Submitted } or { DeletedDate: not null } or { Status: not GraphQL.VacancyStatus.Submitted })
        {
            // it's been transferred/deleted so ignore
            return TypedResults.NoContent();
        }
        
        // Patch the Vacancy
        var patchDocument = new JsonPatchDocument<Recruit.Contracts.ApiResponses.Vacancy>();
        patchDocument.Replace(x => x.Status, VacancyStatus.Approved);
        patchDocument.Replace(x => x.ApprovedDate, DateTime.UtcNow);
        
        var patchResponse = await recruitApiClient.PatchWithResponseCode<JsonPatchDocument<Recruit.Contracts.ApiResponses.Vacancy>, NullResponse>(new PatchVacanciesByVacancyIdApiRequest
        {
            Data = patchDocument,
            VacancyId = vacancyId
        }, false);
        patchResponse.EnsureSuccessStatusCode();
        return TypedResults.NoContent();
    }
    
    [HttpPost, Route("{vacancyId:guid}/publish")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> PublishVacancy(
        [FromServices] IMessageSession messageSession,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        [FromServices] Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration> recruitApiClient,
        [FromRoute] Guid vacancyId,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient.GetVacancyById.ExecuteAsync(vacancyId, cancellationToken);
        if (!response.IsSuccessResult())
        {
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var vacancy = response.Data!.Vacancies.FirstOrDefault();
        if (vacancy is { TransferInfo: not null, Status: not GraphQL.VacancyStatus.Approved } or { DeletedDate: not null } or { Status: not GraphQL.VacancyStatus.Approved })
        {
            // it's been transferred/deleted so ignore
            return TypedResults.NoContent();
        }
        
        // Patch the Vacancy
        var patchDocument = new JsonPatchDocument<SFA.DAS.Recruit.Contracts.ApiResponses.Vacancy>();
        patchDocument.Replace(x => x.Status, VacancyStatus.Live);
        patchDocument.Replace(x => x.LiveDate, DateTime.UtcNow);

        var patchResponse = await recruitApiClient.PatchWithResponseCode<JsonPatchDocument<SFA.DAS.Recruit.Contracts.ApiResponses.Vacancy>, NullResponse>(new PatchVacanciesByVacancyIdApiRequest
        {
            Data = patchDocument,
            VacancyId = vacancyId
        }, false);
        patchResponse.EnsureSuccessStatusCode();
        return TypedResults.NoContent();
    }

    [HttpGet, Route("stale/archive")]
    [ProducesResponseType(typeof(DataResponse<IEnumerable<StaleVacancyIdentifier>>), StatusCodes.Status200OK)]
    public async Task<IResult> GetClosedVacanciesToArchive([FromQuery, Required] DateTime pointInTime, 
        [FromServices] Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration> recruitApiClient,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient
            .GetVacanciesToArchive
            .ExecuteAsync(pointInTime, cancellationToken);

        if (!response.IsSuccessResult())
        {
            logger.LogError("Error in GetClosedVacanciesToArchive: {Errors}", response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var gqlVacancies = response.Data!.Vacancies.Take(10);

        var tasks = gqlVacancies.Select(async v =>
        {
            var reviews = await recruitApiClient.Get<List<GetApplicationReviewResponse>>(new GetApplicationreviewsByVacancyReferenceApiRequest(v.VacancyReference.GetValueOrDefault())
            {
                VacancyReference = v.VacancyReference.GetValueOrDefault()
            });

            var allHaveOutcome = reviews
                .All(x =>
                x.Status is ApplicationReviewStatus.Successful or ApplicationReviewStatus.Unsuccessful || x.WithdrawnDate != null);

            return (v, allHaveOutcome);
        });

        var evaluated = await Task.WhenAll(tasks);

        var result = evaluated
            .Where(x => x.allHaveOutcome && x.v.ClosingDate != null)
            .Select(x => new StaleArchiveVacancyIdentifier(
                x.v.Id,
                x.v.VacancyReference,
                VacancyStatus.Closed,
                x.v.ClosingDate!.Value.UtcDateTime))
            .ToList();

        return TypedResults.Ok(new DataResponse<IEnumerable<StaleArchiveVacancyIdentifier>>(result));
    }

    [HttpPost, Route("{vacancyReference:long}/archive")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IResult> ArchiveVacancy(
        [FromRoute] long vacancyReference,
        [FromBody] ArchiveVacancyRequest request,
        [FromServices] VacancyMapper vacancyMapper,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        [FromServices] Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration> recruitApiClient,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await recruitGqlClient
                .GetVacancyById
                .ExecuteAsync(request.VacancyId, cancellationToken);

            if (!response.IsSuccessResult())
            {
                logger.LogError("An error occured at ArchiveVacancy: {Errors}", response.FormatErrors());
                return TypedResults.Problem(response.ToProblemDetails());
            }

            var vacancy = response.Data!.Vacancies.FirstOrDefault();
            if (vacancy is null 
                || vacancy.VacancyReference != vacancyReference
                || vacancy.Status != GraphQL.VacancyStatus.Closed)
            {
                logger.LogWarning("Vacancy with id {VacancyId} not found at ArchiveVacancy", request.VacancyId);
                return TypedResults.NotFound();
            }

            var domainVacancy = GqlVacancyMapper.ToDomain(vacancy);
            domainVacancy.Status = VacancyStatus.Archived;
            domainVacancy.ArchiveType = Recruit.Contracts.ApiResponses.ArchiveType.Auto;
            domainVacancy.LastUpdatedDate = DateTime.UtcNow;
            domainVacancy.ArchivedDate = DateTime.UtcNow;

            var putResponse = await recruitApiClient.PutWithResponseCode<PutVacancyRequest, SFA.DAS.Recruit.Contracts.ApiResponses.Vacancy>(
                new PutVacanciesByVacancyIdApiRequest
                {
                    VacancyId = request.VacancyId,
                    RuleSet = VacancyRuleSet.All,
                    ValidateOnly = false,
                    Data = vacancyMapper.ToInnerDto(domainVacancy)
                });

            putResponse.EnsureSuccessStatusCode();
            return TypedResults.NoContent();
        }
        catch (ApiResponseException ex)
        {
            logger.LogError(ex, "Error while archiving vacancy: {id}", request.VacancyId);
            return TypedResults.Problem(title: ex.Message, detail: ex.Error);
        }
    }
}