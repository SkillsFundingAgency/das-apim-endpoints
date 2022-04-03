using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadAddAndApproveDraftApprenticeshipsCommandHandler : IRequestHandler<BulkUploadAddAndApproveDraftApprenticeshipsCommand, BulkUploadAddAndApproveDraftApprenticeshipsResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public BulkUploadAddAndApproveDraftApprenticeshipsCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BulkUploadAddAndApproveDraftApprenticeshipsResult> Handle(BulkUploadAddAndApproveDraftApprenticeshipsCommand request, CancellationToken cancellationToken)
        {
           var result = await _apiClient.PostWithResponseCode<BulkUploadAddAndApproveDraftApprenticeshipsResponse>(
                new PostAddAndApproveDraftApprenticeshipsRequest(request.ProviderId,
                 new BulkUploadAddAndApproveDraftApprenticeshipsRequest
                 {
                     BulkUploadAddAndApproveDraftApprenticeships = request.BulkUploadAddAndApproveDraftApprenticeships,
                     ProviderId = request.ProviderId,
                     UserInfo = request.UserInfo
                 }));

            result.EnsureSuccessStatusCode();

            return new BulkUploadAddAndApproveDraftApprenticeshipsResult
            {
                BulkUploadAddAndApproveDraftApprenticeshipResponse = result.Body.BulkUploadAddAndApproveDraftApprenticeshipResponse.Select(x => (BulkUploadAddDraftApprenticeshipsResult)x)
            };
        }
    }
}
