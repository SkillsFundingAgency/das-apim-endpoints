using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddEmail;

public class DraftApprenticeshipAddEmailCommandHandler(
        ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        : IRequestHandler<DraftApprenticeshipAddEmailCommand, Unit>
{
    public async Task<Unit> Handle(DraftApprenticeshipAddEmailCommand request, CancellationToken cancellationToken)
    {
        var setEmailRequest = new DraftApprenticeshipAddEmailRequest(request.DraftApprenticeshipId, request.CohortId);

        setEmailRequest.Data = new DraftApprenticeshipAddEmailRequest.Body()
        {          
            Email = request.Email,   
            CohortId = request.CohortId
        };

        var response = await apiClient.PostWithResponseCode<EmptyResponse>(setEmailRequest, false);
        response.EnsureSuccessStatusCode();

        return Unit.Value;
    }
}
