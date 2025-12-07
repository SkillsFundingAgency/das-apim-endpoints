using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.Reference;

public class DraftApprenticeshipSetReferenceCommandHandler(
        ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        : IRequestHandler<DraftApprenticeshipSetReferenceCommand, DraftApprenticeshipSetReferenceResponse>
{
    public async Task<DraftApprenticeshipSetReferenceResponse> Handle(DraftApprenticeshipSetReferenceCommand request, CancellationToken cancellationToken)
    {
        var setRequest = new DraftApprenticeshipSetReferenceRequest(request.DraftApprenticeshipId, request.CohortId);

        setRequest.Data = new DraftApprenticeshipSetReferenceRequest.Body()
        {
            Reference = request.Reference,
            Party = request.Party,
            CohortId = request.CohortId
        };

        var response = await apiClient.PostWithResponseCode<DraftApprenticeshipSetReferenceResponse>(setRequest, false);
        response.EnsureSuccessStatusCode();
        return response.Body;
    }
}