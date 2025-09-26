using System.ComponentModel.DataAnnotations;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolsSupport.Api.Models.EmployerAccount;
using SFA.DAS.ToolsSupport.Api.sources.EmployerAccount;
using SFA.DAS.ToolsSupport.Application.Commands.ChangeUserRole;
using SFA.DAS.ToolsSupport.Application.Commands.SupportCreateInvitation;
using SFA.DAS.ToolsSupport.Application.Commands.SupportResendInvitation;
using SFA.DAS.ToolsSupport.Application.Queries.GetAccountFinance;
using SFA.DAS.ToolsSupport.Application.Queries.GetAccountOrganisations;
using SFA.DAS.ToolsSupport.Application.Queries.GetEmployerAccountDetails;
using SFA.DAS.ToolsSupport.Application.Queries.GetPayeSchemeLevyDeclarations;
using SFA.DAS.ToolsSupport.Application.Queries.GetTeamMembers;
using SFA.DAS.ToolsSupport.Application.Queries.SearchEmployerAccounts;

namespace SFA.DAS.ToolsSupport.Api.Controllers;

[Route("[controller]/")]
[ApiController]
public class EmployerAccountsController(IMediator mediator, ILogger<EmployerAccountsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] long? accountId, [FromQuery] string? payeSchemeRef, [FromQuery] string? employerName)
    {
        try
        {
            var response = await mediator.Send(new SearchEmployerAccountsQuery { AccountId = accountId, PayeSchemeRef = payeSchemeRef, EmployerName = employerName });
            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error attempting to query Employer Account using accountId {accountId}, payeSchemeRef {payeSchemeRef}, or employerName {employerName}", accountId, payeSchemeRef, employerName);
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("{accountId}/account-details")]
    public async Task<IActionResult> GetAccountDetails(long accountId)
    {
        try
        {
            var result = await mediator.Send(new GetEmployerAccountDetailsQuery
            {
                AccountId = accountId
            });

            return Ok((GetEmployerAccountDetailsResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error calling GetAccountDetails");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("{accountId}/organisations")]
    public async Task<IActionResult> GetAccountOrganisations(long accountId)
    {
        try
        {
            var result = await mediator.Send(new GetAccountOrganisationsQuery
            {
                AccountId = accountId,
            });

            return Ok((GetAccountOrganisationsResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error calling GetAccountOrganisations");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("{accountId}/team-members")]
    public async Task<IActionResult> GetTeamMembers(long accountId)
    {
        try
        {
            var result = await mediator.Send(new GetTeamMembersQuery
            {
                AccountId = accountId,
            });

            return Ok((GetTeamMembersResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error calling GetTeamMembers");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("{accountId}/finance")]
    public async Task<IActionResult> GetAccountFinance(long accountId)
    {
        try
        {
            var result = await mediator.Send(new GetAccountFinanceQuery
            {
                AccountId = accountId,
            });

            return Ok((GetAccountFinanceResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error calling GetAccountFinance");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("{accountId}/paye-levy-declarations")]
    public async Task<IActionResult> GetPayeLevyDeclarations(long accountId, [FromQuery, Required] string payeRef)
    {
        try
        {
            var result = await mediator.Send(new GetPayeSchemeLevyDeclarationsQuery
            {
                AccountId = accountId,
                PayeRef = payeRef,
            });
            return Ok((GetPayeLevyDeclarationsResponse)result);

        }
        catch (Exception e)
        {
            logger.LogError(e, "Error calling GetPayeLevyDeclarations");
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
            logger.LogError(ex, "Error calling SendInvitation");
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
            logger.LogError(ex, "Error calling ResendInvitation");
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
            logger.LogError(ex, "Error calling ChangeUserRole");
            return BadRequest();
        }
    }
}