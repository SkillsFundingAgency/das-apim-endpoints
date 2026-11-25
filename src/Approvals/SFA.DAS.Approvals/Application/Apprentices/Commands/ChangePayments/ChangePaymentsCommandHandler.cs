using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Commands.ChangePayments;

public class ChangePaymentsCommandHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient,
    ServiceParameters serviceParameters)
    : IRequestHandler<ChangePaymentsCommand, Unit>
{
    public async Task<Unit> Handle(ChangePaymentsCommand request, CancellationToken cancellationToken)
    {
        var patchBody = new PatchApprenticeshipPaymentsApiRequest.Body
        {
            UserInfo = request.UserInfo,
            PaymentFreezeDate = request.PaymentFreezeDate,
            FreezePaymentsReason = request.FreezePaymentsReason,
            Party = (int)(serviceParameters.CallingParty != Shared.Enums.Party.None
                ? serviceParameters.CallingParty
                : Shared.Enums.Party.None)
        };

        var response = await commitmentsApiClient.PatchWithResponseCode<PatchApprenticeshipPaymentsApiRequest.Body, NullResponse>(
            new PatchApprenticeshipPaymentsApiRequest(request.ApprenticeshipId, patchBody),
            false);

        response.EnsureSuccessStatusCode();

        return Unit.Value;
    }
}
