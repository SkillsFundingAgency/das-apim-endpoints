using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.Application.BlockedOrganisations.Commands.UpsertBlockedOrganisation;
using SFA.DAS.RecruitQa.Application.BlockedOrganisations.Queries.GetBlockedOrganisationByOrganisationId;
using SFA.DAS.RecruitQa.Application.BlockedOrganisations.Queries.GetBlockedOrganisationsByOrganisationType;
using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.Api.Controllers;

[Route("[controller]s")]
[ApiController]
public class BlockedOrganisationController(IMediator mediator, ILogger<BlockedOrganisationController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{organisationId}")]
    [ProducesResponseType(typeof(BlockedOrganisationRequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IResult> GetBlockedOrganisation(string organisationId, CancellationToken token)
    {
        try
        {
            var result = await mediator.Send(new GetBlockedOrganisationByOrganisationIdQuery(organisationId), token);

            if (result?.BlockedOrganisation == null)
            {
                return Results.NotFound();
            }

            return TypedResults.Ok((BlockedOrganisationRequestDto)result.BlockedOrganisation);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting blocked organisation for {OrganisationId}", organisationId);
            return Results.Problem(statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    [Route("")]
    public async Task<IResult> UpdateBlockedOrganisation([FromBody] BlockedOrganisationRequestDto blockedOrganisation, CancellationToken token)
    {
        try
        {
            await mediator.Send(new UpsertBlockedOrganisationCommand
            {
                Id = blockedOrganisation.Id,
                BlockedStatus = blockedOrganisation.BlockedStatus,
                OrganisationId = blockedOrganisation.OrganisationId,
                OrganisationType = blockedOrganisation.OrganisationType,
                UpdatedByUserEmail = blockedOrganisation.UpdatedByUserEmail,
                UpdatedByUserId = blockedOrganisation.UpdatedByUserId,
                Reason = blockedOrganisation.Reason
            }, token);

            return TypedResults.Created();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating blocked organisation for {OrganisationId}", blockedOrganisation.OrganisationId);
            return Results.Problem(statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }
    
    [HttpGet]
    [Route("byorganisationtype/{organisationType}")]
    [ProducesResponseType(typeof(List<BlockedOrganisationRequestDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IResult> GetManyByOrganisationType(string organisationType, CancellationToken token)
    {
        try
        {
            var result = await mediator.Send(new GetBlockedOrganisationsByOrganisationTypeQuery
            {
                OrganisationType = organisationType
            }, token);

            return TypedResults.Ok(result.BlockedOrganisations.Select(c => (BlockedOrganisationRequestDto)c).ToList());
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting blocked organisations for type {organisationType}", organisationType);
            return Results.Problem(statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }
}