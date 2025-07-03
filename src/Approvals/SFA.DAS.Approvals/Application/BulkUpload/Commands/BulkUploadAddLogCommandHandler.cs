using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadAddLogCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        : IRequestHandler<BulkUploadAddLogCommand, BulkUploadAddLogResult>
    {
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

            var result = await apiClient.PostWithResponseCode<GetBulkUploadAddLogResponse>(
                new PostBulkUploadAddLogRequest(command.ProviderId, dataToSend));

            result.EnsureSuccessStatusCode();

            return new BulkUploadAddLogResult
            {
                LogId = result.Body.LogId
            };
        }
    }
}