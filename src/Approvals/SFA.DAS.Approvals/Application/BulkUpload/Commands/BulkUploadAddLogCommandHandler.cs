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
    public class BulkUploadAddLogCommandHandler : IRequestHandler<BulkUploadAddLogCommand, BulkUploadAddLogResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public BulkUploadAddLogCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BulkUploadAddLogResult> Handle(BulkUploadAddLogCommand command, CancellationToken cancellationToken)
        {
            var dataToSend = new BulkUploadAddLogRequest
            {
                ProviderId = command.ProviderId,
                FileName = command.FileName,
                RplCount = command.RplCount,
                RowCount = command.RowCount,
                FileContent = command.FileContent,
                UserInfo = command.UserInfo
            };

            var result = await _apiClient.PostWithResponseCode<GetBulkUploadAddLogResponse>(
                new PostBulkUploadAddLogRequest(command.ProviderId, dataToSend));

            result.EnsureSuccessStatusCode();

            return new BulkUploadAddLogResult
            {
                LogId = result.Body.LogId
            };
        }
    }
}