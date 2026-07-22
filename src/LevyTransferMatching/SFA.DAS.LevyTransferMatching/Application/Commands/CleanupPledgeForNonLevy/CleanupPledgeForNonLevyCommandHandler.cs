using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models.Constants;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CleanupPledgeForNonLevy;

public class CleanupPledgeForNonLevyCommandHandler(
    ILevyTransferMatchingService levyTransferMatchingService,
    ILogger<CleanupPledgeForNonLevyCommandHandler> logger)
    : IRequestHandler<CleanupPledgeForNonLevyCommand, Unit>
{
    public async Task<Unit> Handle(CleanupPledgeForNonLevyCommand request, CancellationToken cancellationToken)
    {
        var pledge = await levyTransferMatchingService.GetPledge(request.PledgeId);

        if (pledge == null)
        {
            logger.LogWarning("Pledge {PledgeId} not found for account {AccountId}", request.PledgeId, request.AccountId);
            return Unit.Value;
        }

        if (pledge.AccountId != request.AccountId)
        {
            logger.LogWarning(
                "Pledge {PledgeId} belongs to account {PledgeAccountId}, not event account {AccountId}. Skipping.",
                request.PledgeId,
                pledge.AccountId,
                request.AccountId);
            return Unit.Value;
        }

        if (pledge.Status == PledgeStatus.Closed)
        {
            logger.LogInformation("Pledge {PledgeId} already closed for account {AccountId}", request.PledgeId, request.AccountId);
            return Unit.Value;
        }

        await RejectApplicationsByStatus(request, ApplicationStatus.Pending);
        await RejectApprovedApplications(request);

        var closeResponse = await levyTransferMatchingService.ClosePledge(
            new ClosePledgeRequest(
                request.PledgeId,
                new ClosePledgeRequest.ClosePledgeRequestData
                {
                    UserId = string.Empty,
                    UserDisplayName = string.Empty
                }));

        if (!closeResponse.StatusCode.IsSuccess())
        {
            throw new InvalidOperationException(
                $"Unable to close pledge {request.PledgeId}. StatusCode: {closeResponse.StatusCode}, Error: {closeResponse.ErrorContent}");
        }

        logger.LogInformation("Completed non-levy cleanup for pledge {PledgeId}, account {AccountId}", request.PledgeId, request.AccountId);

        return Unit.Value;
    }

    private async Task RejectApprovedApplications(CleanupPledgeForNonLevyCommand request)
    {
        var approvedApplications = await levyTransferMatchingService.GetApplications(new GetApplicationsRequest
        {
            PledgeId = request.PledgeId,
            SenderAccountId = request.AccountId,
            ApplicationStatusFilter = ApplicationStatus.Approved
        });

        foreach (var application in approvedApplications?.Applications ?? [])
        {
            var undoResponse = await levyTransferMatchingService.UndoApplicationApproval(
                new UndoApplicationApprovalRequest(request.PledgeId, application.Id));

            if (!undoResponse.StatusCode.IsSuccess())
            {
                throw new InvalidOperationException(
                    $"Unable to undo approval for application {application.Id} on pledge {request.PledgeId}. StatusCode: {undoResponse.StatusCode}, Error: {undoResponse.ErrorContent}");
            }

            await levyTransferMatchingService.RejectApplication(new RejectApplicationRequest(
                request.PledgeId,
                application.Id,
                new RejectApplicationRequestData
                {
                    UserId = string.Empty,
                    UserDisplayName = string.Empty
                }));
        }
    }

    private async Task RejectApplicationsByStatus(CleanupPledgeForNonLevyCommand request, string status)
    {
        var applications = await levyTransferMatchingService.GetApplications(new GetApplicationsRequest
        {
            PledgeId = request.PledgeId,
            SenderAccountId = request.AccountId,
            ApplicationStatusFilter = status
        });

        foreach (var application in applications?.Applications ?? [])
        {
            await levyTransferMatchingService.RejectApplication(new RejectApplicationRequest(
                request.PledgeId,
                application.Id,
                new RejectApplicationRequestData
                {
                    UserId = string.Empty,
                    UserDisplayName = string.Empty
                }));
        }
    }
}
