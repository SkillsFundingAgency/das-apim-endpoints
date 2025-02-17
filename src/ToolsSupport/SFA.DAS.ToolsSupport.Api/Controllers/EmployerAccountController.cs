using System.ComponentModel.DataAnnotations;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolsSupport.Api.Models.EmployerAccount;
using SFA.DAS.ToolsSupport.Application.Commands.ChangeUserRole;
using SFA.DAS.ToolsSupport.Application.Commands.SupportCreateInvitation;
using SFA.DAS.ToolsSupport.Application.Commands.SupportResendInvitation;
using SFA.DAS.ToolsSupport.Application.Queries.GetEmployerAccountDetails;
using SFA.DAS.ToolsSupport.Models.Constants;

namespace SFA.DAS.ToolsSupport.Api.Controllers;

[Route("[controller]/")]
[ApiController]
public class EmployerAccountController(IMediator mediator, ILogger<EmployerAccountController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{accountId}/account-details")]
    public async Task<IActionResult> GetAccountDetails(long accountId, [FromQuery, Required] AccountFieldSelection accountFieldSelection)
    {
        try
        {
            var result = await mediator.Send(new GetEmployerAccountDetailsQuery
            {
                AccountId = accountId,
                SelectedField = accountFieldSelection
            });

            return Ok((GetEmployerAccountDetailsResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError("Error: GetAccountDetails: {error}", e);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    [Route("send-invitation")]
    public async Task<IActionResult> SendInvitation([FromBody] SupportCreateInvitationRequest request)
    {
        try
        {
            logger.LogInformation("SendInvitation starting for AccountId {Id}", request.HashedAccountId);

            var command = new SupportCreateInvitationCommand
            {
                HashedAccountId = request.HashedAccountId,
                FullName = request.FullName,
                Email = request.Email,
                Role = request.Role
            };

            var responseStatus = await mediator.Send(command);
            if (responseStatus != HttpStatusCode.OK)
            {
                return BadRequest();
            }
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in SendInvitation for AccountId Id: {Id}", request.HashedAccountId);
            return BadRequest();
        }
    }

    [HttpPost]
    [Route("resend-invitation")]
    public async Task<IActionResult> ResendInvitation([FromBody] SupportResendInvitationRequest request)
    {
        try
        {
            logger.LogInformation("ResendInvitation starting for AccountId {Id}", request.HashedAccountId);

            var command = new SupportResendInvitationCommand
            {
                HashedAccountId = request.HashedAccountId,
                Email = Uri.UnescapeDataString(request.Email)
            };

            var responseStatus = await mediator.Send(command);
            if (responseStatus != HttpStatusCode.OK)
            {
                return BadRequest();
            }
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in ResendInvitation for AccountId Id: {Id}", request.HashedAccountId);
            return BadRequest();
        }
    }

    [HttpPost]
    [Route("change-role")]
    public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleRequest request)
    {
        try
        {
            logger.LogInformation("ChangeUserRole starting for AccountId {Id}", request.HashedAccountId);

            var command = new ChangeUserRoleCommand
            {
                HashedAccountId = request.HashedAccountId,
                Email = Uri.UnescapeDataString(request.Email),
                Role = request.Role
            };

            var responseStatus = await mediator.Send(command);
            if (responseStatus != HttpStatusCode.OK)
            {
                return BadRequest();
            }
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in ChangeUserRole for AccountId Id: {Id}", request.HashedAccountId);
            return BadRequest();
        }
    }
}