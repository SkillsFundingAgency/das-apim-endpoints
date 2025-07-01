using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadLogUpdateWithErrorContentCommandHandler(
        ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        : IRequestHandler<BulkUploadLogUpdateWithErrorContentCommand, Unit>
    {
        public async Task<Unit> Handle(BulkUploadLogUpdateWithErrorContentCommand command, CancellationToken cancellationToken)
        {
            var dataToSend = new BulkUploadLogUpdateWithErrorContentRequest
            {
                ErrorContent = command.ErrorContent,
                UserInfo = command.UserInfo
            };

            await apiClient.Put(new PutBulkUploadLogUpdateWithErrorContentRequest(command.ProviderId, command.LogId, dataToSend));

            return Unit.Value;
        }
    }
}
