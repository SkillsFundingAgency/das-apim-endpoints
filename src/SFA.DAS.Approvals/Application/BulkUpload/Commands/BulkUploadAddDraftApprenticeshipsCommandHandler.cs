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
    public class BulkUploadAddDraftApprenticeshipsCommandHandler : IRequestHandler<BulkUploadAddDraftApprenticeshipsCommand, GetBulkUploadAddDraftApprenticeshipsResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public BulkUploadAddDraftApprenticeshipsCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetBulkUploadAddDraftApprenticeshipsResult> Handle(BulkUploadAddDraftApprenticeshipsCommand request, CancellationToken cancellationToken)
        {
           var result = await _apiClient.PostWithResponseCode<GetBulkUploadAddDraftApprenticeshipsResponse>(
                new PostAddDraftApprenticeshipsRequest(request.ProviderId,
                 new BulkUploadAddDraftApprenticeshipsRequest
                 {
                     BulkUploadDraftApprenticeships = request.BulkUploadAddDraftApprenticeships,
                     ProviderId = request.ProviderId,
                     UserInfo = request.UserInfo
                 }));
            
            result.EnsureSuccessStatusCode();

            return new GetBulkUploadAddDraftApprenticeshipsResult
            {
                BulkUploadAddDraftApprenticeshipsResponse = result.Body.BulkUploadAddDraftApprenticeshipsResponse.Select(x => (BulkUploadAddDraftApprenticeshipsResult)x)
            };
        }
    }
}
