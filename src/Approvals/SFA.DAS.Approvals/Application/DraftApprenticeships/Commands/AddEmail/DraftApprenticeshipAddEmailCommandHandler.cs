using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
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