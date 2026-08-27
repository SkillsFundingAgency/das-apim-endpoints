using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.ApprovalRequest.Commands;
using SFA.DAS.Approvals.Application.ApprovalRequest.Queries;

namespace SFA.DAS.Approvals.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ApprovalRequestController(IMediator mediator, ILogger<ChangeHistoryController> logger) : ControllerBase
{
    [HttpGet]
    [Route("apprenticeships/{apprenticeshipId:long}")]
    public async Task<IActionResult> GetApprovalRequest(long apprenticeshipId, [FromQuery] byte status)
    {
        try
        {
            var queryResult = await mediator.Send(new GetApprovalRequestQuery
            {
                ApprenticeshipId = apprenticeshipId,
                Status = status
            });

            var model = queryResult;
            return Ok(model);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error attempting to get approval request for apprenticeshipId: {ApprenticeshipId}", apprenticeshipId);
            return BadRequest();
        }
    }

    [HttpPut]
    [Route("apprenticeships/{apprenticeshipId:long}/alerts-acknowledged")]
    public async Task<IActionResult> UpdateApprovalRequestAlertAcknowledge(long apprenticeshipId, [FromBody] UpdateApprovalRequestAlertAcknowledgeRequest request)
    {
        try
        {
            var command = new UpdateApprovalRequestAlertAcknowledgeCommand
            {
                ApprenticeshipId = apprenticeshipId,
                ApprovalRequestAlerts = request.ApprovalRequestAlerts
            };
            await mediator.Send(command);
            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error attempting to update approval request for apprenticeshipId: {ApprenticeshipId}", apprenticeshipId);
            return BadRequest();
        }
    }
}