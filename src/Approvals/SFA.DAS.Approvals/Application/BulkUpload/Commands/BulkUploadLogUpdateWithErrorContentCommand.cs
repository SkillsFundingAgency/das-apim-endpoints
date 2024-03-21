using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadLogUpdateWithErrorContentCommand : IRequest<Unit>
    {
        public long LogId { get; set; }
        public long ProviderId { get; set; }
        public string ErrorContent { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}