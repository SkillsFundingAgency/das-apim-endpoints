using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries
{
    public class GetDraftApprenticeshipsQueryHandler : IRequestHandler<GetDraftApprenticeshipsQuery, GetDraftApprenticeshipsResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public GetDraftApprenticeshipsQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async  Task<GetDraftApprenticeshipsResult> Handle(GetDraftApprenticeshipsQuery request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.Get<GetDraftApprenticeshipsResponse>(new GetDraftApprenticeshipsRequest(request.CohortId));

            if (response == null)
                return null;

            return new GetDraftApprenticeshipsResult
            {
                DraftApprenticeships = response.DraftApprenticeships.Select(x => (DraftApprenticeship)x).ToList()
            };
        }
    }
}
