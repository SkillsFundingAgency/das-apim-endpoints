using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadLogUpdateWithErrorContentCommandHandler : IRequestHandler<BulkUploadLogUpdateWithErrorContentCommand, Unit>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public BulkUploadLogUpdateWithErrorContentCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<Unit> Handle(BulkUploadLogUpdateWithErrorContentCommand command, CancellationToken cancellationToken)
        {
            var dataToSend = new BulkUploadLogUpdateWithErrorContentRequest
            {
                ErrorContent = command.ErrorContent,
                UserInfo = command.UserInfo
            };

            await _apiClient.Put(new PutBulkUploadLogUpdateWithErrorContentRequest(command.ProviderId, command.LogId, dataToSend));

            return Unit.Value;
        }
    }
}
