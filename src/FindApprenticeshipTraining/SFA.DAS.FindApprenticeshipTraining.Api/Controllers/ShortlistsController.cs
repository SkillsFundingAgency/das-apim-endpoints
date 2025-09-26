using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipTraining.Api.ApiRequests;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistItem;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistCountForUser;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistsForUser;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ShortlistsController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    [Route("")]
    [ProducesResponseType<PostShortListResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateShortlistForUser(CreateShortListRequest shortlistRequest)
    {
        PostShortListResponse result = await _mediator.Send(
            new CreateShortlistForUserCommand
            {
                Ukprn = shortlistRequest.Ukprn,
                LocationName = shortlistRequest.LocationName,
                LarsCode = shortlistRequest.LarsCode,
                ShortlistUserId = shortlistRequest.ShortlistUserId
            });

        return Ok(result);
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(DeleteShortlistItemCommandResult))]
    public async Task<IActionResult> DeleteShortlistItemForUser(Guid id)
    {
        var result = await _mediator.Send(new DeleteShortlistItemCommand(id));
        return Accepted(result);
    }

    [HttpGet]
    [Route("users/{userId}/count")]
    [ProducesResponseType<GetShortlistCountForUserQueryResult>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetShortlistCountForUser(Guid userId)
    {
        GetShortlistCountForUserQueryResult result = await _mediator.Send(new GetShortlistCountForUserQuery(userId));
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
}
