using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class ValidateBulkUploadRecordsCommandHandler : IRequestHandler<ValidateBulkUploadRecordsCommand>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public ValidateBulkUploadRecordsCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<Unit> Handle(ValidateBulkUploadRecordsCommand request, CancellationToken cancellationToken)
        {
            await _apiClient.PostWithResponseCode<object>(new PostValidateBulkUploadRequest(request.ProviderId,
                new BulkUploadAddDraftApprenticeshipsRequest 
                { 
                     CsvRecords = request.CsvRecords,
                     ProviderId = request.ProviderId,
                     UserInfo = request.UserInfo
                }));
            return Unit.Value;
        }
    }
}
