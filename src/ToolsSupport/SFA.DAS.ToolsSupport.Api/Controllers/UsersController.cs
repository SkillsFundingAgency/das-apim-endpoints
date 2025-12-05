using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.ToolsSupport.Api.Models.Users;
using SFA.DAS.ToolsSupport.Application.Commands.EmployerUsers;
using System.Net;
using ApiChangeUserStatusResponse = SFA.DAS.ToolsSupport.Api.Models.Users.ChangeUserStatusResponse;
using InnerChangeUserStatusResponse = SFA.DAS.ToolsSupport.InnerApi.Responses.ChangeUserStatusResponse;

namespace SFA.DAS.ToolsSupport.Api.Controllers;

[ApiController]
[Route("users")]
public class UsersController(IMediator mediator, ILogger<UsersController> logger) : ControllerBase
{
    [HttpPost("{identifier}/suspend")]
    public async Task<IActionResult> SuspendUser(string identifier, [FromBody] ChangeUserStatusRequest request)
    {
        return await ChangeUserStatus(identifier, request, suspend: true);
    }

    [HttpPost("{identifier}/resume")]
    public async Task<IActionResult> ResumeUser(string identifier, [FromBody] ChangeUserStatusRequest request)
    {
        return await ChangeUserStatus(identifier, request, suspend: false);
    }

    private async Task<IActionResult> ChangeUserStatus(string identifier, ChangeUserStatusRequest request, bool suspend)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            ApiResponse<InnerChangeUserStatusResponse> response = suspend
                ? await mediator.Send(new SuspendEmployerUserCommand(identifier, request.ChangedByUserId, request.ChangedByEmail))
                : await mediator.Send(new ResumeEmployerUserCommand(identifier, request.ChangedByUserId, request.ChangedByEmail));

            return MapResponse(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error attempting to change user suspension state for identifier {Identifier}", identifier);
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    private static IActionResult MapResponse(ApiResponse<InnerChangeUserStatusResponse> response)
    {
        if (response == null)
        {
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new NotFoundResult();
        }

        if (response is { StatusCode: HttpStatusCode.BadRequest, Body: not null })
        {
            return new OkObjectResult(ApiChangeUserStatusResponse.FromInnerResponse(response.Body));
        }

        return new ObjectResult(ApiChangeUserStatusResponse.FromInnerResponse(response.Body))
        {
            StatusCode = (int)response.StatusCode
        };
    }
}

