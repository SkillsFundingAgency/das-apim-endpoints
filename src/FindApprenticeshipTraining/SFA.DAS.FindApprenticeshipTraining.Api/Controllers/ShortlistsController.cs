using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeshipTraining.Api.ApiRequests;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistItem;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetExpiredShortlists;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistCountForUser;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistsForUser;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ShortlistsController(IMediator _mediator, ILogger<ShortlistsController> _logger) : ControllerBase
{
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateShortlistForUser(CreateShortListRequest shortlistRequest)
    {
        PostShortListResponse result = await _mediator.Send(
            new CreateShortlistForUserCommand
            {
                Lat = shortlistRequest.Lat,
                Lon = shortlistRequest.Lon,
                Ukprn = shortlistRequest.Ukprn,
                LocationDescription = shortlistRequest.LocationDescription,
                LarsCode = shortlistRequest.LarsCode,
                ShortlistUserId = shortlistRequest.ShortlistUserId
            });

        return CreatedAtAction(nameof(GetShortlistsForUser), new { UserId = shortlistRequest.ShortlistUserId }, result);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteShortlistItemForUser(Guid id)
    {
        await _mediator.Send(new DeleteShortlistItemCommand
        {
            ShortlistId = id
        });
        return Accepted();
    }

    [HttpGet]
    [Route("users/{userId}/count")]
    public async Task<IActionResult> GetShortlistCountForUser(Guid userId)
    {
        var result = await _mediator.Send(new GetShortlistCountForUserQuery(userId));
        return Ok(result);
    }

    [HttpGet]
    [Route("users/{userId}")]
    [ProducesResponseType<GetShortlistsForUserResponse>(200)]
    public async Task<IActionResult> GetShortlistsForUser(Guid userId)
    {
        GetShortlistsForUserResponse result = await _mediator.Send(new GetShortlistsForUserQuery { UserId = userId });
        return Ok(result);
    }

    [HttpGet]
    [Route("expired")]
    public async Task<IActionResult> GetExpiredShortlistUserIds([FromQuery] uint expiryInDays)
    {
        try
        {
            var result = await _mediator.Send(new GetExpiredShortlistsQuery { ExpiryInDays = expiryInDays });

            return Ok(new { result.UserIds });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting list of expired shortlists");
            return BadRequest();
        }
    }

    [HttpDelete]
    [Route("users/{userId}")]
    public async Task<IActionResult> DeleteShortlistForUser(Guid userId)
    {
        try
        {
            await _mediator.Send(new DeleteShortlistForUserCommand
            {
                UserId = userId
            });
            return Accepted();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting shortlist");
            return BadRequest();
        }
    }
}
