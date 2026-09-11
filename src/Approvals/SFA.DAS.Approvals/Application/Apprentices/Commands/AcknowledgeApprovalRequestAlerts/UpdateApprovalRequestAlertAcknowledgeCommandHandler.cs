using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Commands.AcknowledgeApprovalRequestAlerts;

public class UpdateApprovalRequestAlertAcknowledgeCommandHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient) : IRequestHandler<UpdateApprovalRequestAlertAcknowledgeCommand>
{
    public async Task Handle(UpdateApprovalRequestAlertAcknowledgeCommand request, CancellationToken cancellationToken)
    {
        var response = await commitmentsV2ApiClient.
            PutWithResponseCode<NullResponse>(new UpdateApprovalRequestAlertAcknowledgeRequest(request.ApprenticeshipId,
            new Body
            {
                AccountId = request.AccountId,
                ApprovalRequestAlerts = request.ApprovalRequestAlerts.Select(r => new UpdateApprovalRequestAlertAcknowledgeItem
                {
                    ApprovalRequestId = r.ApprovalRequestId,
                    Acknowledged = r.Acknowledged,
                    UserInfo = r.UserInfo
                }).ToList()
            }));

        response.EnsureSuccessStatusCode();
    }
}