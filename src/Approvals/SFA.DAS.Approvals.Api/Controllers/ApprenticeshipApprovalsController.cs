using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.ApprenticeshipApprovals.Query;
namespace SFA.DAS.Approvals.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ApprenticeshipApprovalsController(
    ILogger<ApprenticeshipApprovalsController> logger,
    IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Route("/employers/{accountId:long}/apprenticeships/{apprenticeshipId:long}/approvals/{approvalRequestId:guid}")]
    public async Task<IActionResult> GetApprenticeshipApproval(long accountId, long apprenticeshipId, Guid approvalRequestId)
    {
        try
        {
            var result = await mediator.Send(new GetApprenticeshipApprovalQuery { EmployerAccountId = accountId, ApprovalRequestId = approvalRequestId, ApprenticeshipId = apprenticeshipId });

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        catch (UnauthorizedAccessException e)
        {
            logger.LogError(e, "Access denied in GetApprenticeshipApproval {apprenticeshipId} for account {accountId}", apprenticeshipId, accountId);
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error in GetApprenticeshipApproval {apprenticeshipId}", apprenticeshipId);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}