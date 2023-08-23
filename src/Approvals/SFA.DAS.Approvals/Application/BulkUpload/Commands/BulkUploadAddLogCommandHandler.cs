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
        private readonly IMediator _mediator;

        public BulkUploadAddLogCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IMediator mediator)
        {
            _apiClient = apiClient;
            _mediator = mediator;
        }

        public async Task<BulkUploadAddLogResult> Handle(BulkUploadAddLogCommand command, CancellationToken cancellationToken)
        {
            var dataToSend = new BulkUploadAddLogRequest
            {
                ProviderId = command.ProviderId,
                FileName = command.FileName,
                RplCount = command.RplCount,
                RowCount = command.RowCount,
                FileContent = command.FileContent
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
