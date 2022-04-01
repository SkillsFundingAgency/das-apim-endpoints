using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadAddAndApproveDraftApprenticeshipsCommandHandler : IRequestHandler<BulkUploadAddAndApproveDraftApprenticeshipsCommand, BulkUploadAddAndApproveDraftApprenticeshipsResponse>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public BulkUploadAddAndApproveDraftApprenticeshipsCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BulkUploadAddAndApproveDraftApprenticeshipsResponse> Handle(BulkUploadAddAndApproveDraftApprenticeshipsCommand request, CancellationToken cancellationToken)
        {
           var result = await _apiClient.PostWithResponseCode<BulkUploadAddAndApproveDraftApprenticeshipsResponse>(
                new PostAddAndApproveDraftApprenticeshipsRequest(request.ProviderId,
                 new BulkUploadAddAndApproveDraftApprenticeshipsRequest
                 {
                     BulkUploadAddAndApproveDraftApprenticeships = request.BulkUploadAddAndApproveDraftApprenticeships,
                     ProviderId = request.ProviderId,
                     UserInfo = request.UserInfo
                 }));
            return result.Body;
        }
    }
}
