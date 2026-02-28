using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.Reference;

public class DraftApprenticeshipSetReferenceCommandHandler(
        ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
        ServiceParameters serviceParameters)
        : IRequestHandler<DraftApprenticeshipSetReferenceCommand>
{
    public async Task Handle(DraftApprenticeshipSetReferenceCommand request, CancellationToken cancellationToken)
    {
        var setRequest = new DraftApprenticeshipSetReferenceRequest(request.DraftApprenticeshipId, request.CohortId);

        setRequest.Data = new DraftApprenticeshipSetReferenceRequest.Body()
        {
            Reference = request.Reference,
            Party = serviceParameters.CallingParty,
        };

        await apiClient.PutWithResponseCode<NullResponse>(setRequest);
    }   
}