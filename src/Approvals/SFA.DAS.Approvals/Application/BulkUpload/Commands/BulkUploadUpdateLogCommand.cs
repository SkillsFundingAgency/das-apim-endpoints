using MediatR;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadUpdateLogCommand : IRequest<BulkUploadUpdateLogResult>
    {
        public long? LogId { get; set; }
    }
}
