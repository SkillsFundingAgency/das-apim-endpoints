namespace SFA.DAS.Approvals.Api.Models
{
    public class BulkUploadAddLogRequest
    {
        public BulkUploadAddLogRequest()
        {
        }
        public long ProviderId { get; set; }
        public string FileName { get; set; }
        public int? RplCount { get; set; }
        public int? RowCount { get; set; }
        public string FileContent { get; set; }
    }
}
