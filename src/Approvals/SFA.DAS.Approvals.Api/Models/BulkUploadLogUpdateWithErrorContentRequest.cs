using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Api.Models
{
    public class BulkUploadLogUpdateWithErrorContentRequest
    {
        public long ProviderId { get; set; }
        public string ErrorContent { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
