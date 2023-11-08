using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Api.Models
{
    public class BulkUploadAddLogRequest
    {
        public long ProviderId { get; set; }
        public string FileName { get; set; }
        public int? RplCount { get; set; }
        public int? RowCount { get; set; }
        public string FileContent { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
