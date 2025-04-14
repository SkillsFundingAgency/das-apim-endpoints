﻿using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeshipTraining.Api.ApiRequests;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistItem;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetExpiredShortlists;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using GetShortlistForUserResponse = SFA.DAS.FindApprenticeshipTraining.Api.Models.GetShortlistForUserResponse;
using GetShortlistItem = SFA.DAS.FindApprenticeshipTraining.Api.Models.GetShortlistItem;

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
                StandardId = shortlistRequest.StandardId,
                ShortlistUserId = shortlistRequest.ShortlistUserId
            });

        return CreatedAtAction(nameof(GetAllForUser), new { UserId = shortlistRequest.ShortlistUserId }, result);
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
    [Route("users/{userId}")]
    public async Task<IActionResult> GetAllForUser(Guid userId)
    {
        try
        {
            var result = await _mediator.Send(new GetShortlistForUserQuery { ShortlistUserId = userId });

            var shortlist = result.Shortlist.OrderBy(x => x.ProviderDetails.DeliveryModelsShortestDistance)
                .ThenByDescending(x => x.ProviderDetails.DeliveryModels.Any(d => d.LocationType == LocationType.National));

            var response = new GetShortlistForUserResponse
            {
                Shortlist = shortlist.Select(item => (GetShortlistItem)item)
            };
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error attempting to get shortlist for user:{UserId}", userId);
            return BadRequest();
        }
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