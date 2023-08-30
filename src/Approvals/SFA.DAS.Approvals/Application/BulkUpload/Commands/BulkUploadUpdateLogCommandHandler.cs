using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadUpdateLogCommandHandler : IRequestHandler<BulkUploadUpdateLogCommand, BulkUploadUpdateLogResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public BulkUploadUpdateLogCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BulkUploadUpdateLogResult> Handle(BulkUploadUpdateLogCommand command, CancellationToken cancellationToken)
        {
            var dataToSend = new BulkUploadUpdateLogRequest
            {
                ProviderId = command.ProviderId,
                LogId = command.LogId
            };

            var result = await _apiClient.PostWithResponseCode<GetBulkUploadUpdateLogResponse>(
                new PostBulkUploadUpdateLogRequest(command.ProviderId, dataToSend));

            result.EnsureSuccessStatusCode();

            return new BulkUploadUpdateLogResult
            {
                LogId = result.Body.LogId
            };
        }
    }
}