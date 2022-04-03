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

        public async Task<Unit> Handle(ValidateBulkUploadRecordsCommand command, CancellationToken cancellationToken)
        {
            // If any errors this call will throw a bulkupload domain exception, which is handled through middleware.
           await _apiClient.PostWithResponseCode<object>(new PostValidateBulkUploadRequest(command.ProviderId,
                new BulkUploadValidateApiRequest 
                { 
                     CsvRecords = command.CsvRecords,
                     ProviderId = command.ProviderId,
                     UserInfo = command.UserInfo
                }));
            return Unit.Value;
        }
    }
}
