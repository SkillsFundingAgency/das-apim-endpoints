using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Commands.ProcessApprenticeshipApproval;

public class ProcessApprenticeshipApprovalCommandHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient)
    : IRequestHandler<ProcessApprenticeshipApprovalCommand, Unit>
{
    public async Task<Unit> Handle(ProcessApprenticeshipApprovalCommand request, CancellationToken cancellationToken)
    {
        var body = new ProcessApprenticeshipApprovalRequest.Body
        {
            UserInfo = request.UserInfo,
            ApplyChanges = request.ApplyChanges
        };

        var response = await commitmentsApiClient.PostWithResponseCode<NullResponse>(
            new ProcessApprenticeshipApprovalRequest(request.ApprenticeshipId, request.ApprovalRequestId, body),
            false);

        response.EnsureSuccessStatusCode();

        return Unit.Value;
    }
}
