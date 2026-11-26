using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddEmail;

public class DraftApprenticeshipAddEmailCommandHandler(
        ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
        ServiceParameters serviceParameters)
        : IRequestHandler<DraftApprenticeshipAddEmailCommand>
{
    public async Task Handle(DraftApprenticeshipAddEmailCommand request, CancellationToken cancellationToken)
    {
        var setEmailRequest = new DraftApprenticeshipAddEmailRequest(request.DraftApprenticeshipId, request.CohortId);

        setEmailRequest.Data = new DraftApprenticeshipAddEmailRequest.Body()
        {
            Email = request.Email,
            Party = serviceParameters.CallingParty
        };

        await apiClient.PutWithResponseCode<NullResponse>(setEmailRequest);
    }
}