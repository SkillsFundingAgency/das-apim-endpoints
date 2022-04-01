using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadAddDraftApprenticeshipsCommandHandler : IRequestHandler<BulkUploadAddDraftApprenticeshipsCommand, GetBulkUploadAddDraftApprenticeshipsResponse>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public BulkUploadAddDraftApprenticeshipsCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetBulkUploadAddDraftApprenticeshipsResponse> Handle(BulkUploadAddDraftApprenticeshipsCommand request, CancellationToken cancellationToken)
        {
           var result = await _apiClient.PostWithResponseCode<GetBulkUploadAddDraftApprenticeshipsResponse>(
                new PostAddDraftApprenticeshipsRequest(request.ProviderId,
                 new BulkUploadAddDraftApprenticeshipsRequest
                 {
                     BulkUploadDraftApprenticeships = request.BulkUploadAddDraftApprenticeships,
                     ProviderId = request.ProviderId,
                     UserInfo = request.UserInfo
                 }));
            return result.Body;
        }
    }
}
