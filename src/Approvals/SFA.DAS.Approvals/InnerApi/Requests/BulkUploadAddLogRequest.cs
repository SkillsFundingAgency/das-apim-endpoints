namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class BulkUploadAddLogRequest : SaveDataRequest
    {
        public long ProviderId { get; set; }
        public string FileName { get; set; }
        public int? RplCount { get; set; }
        public int? RowCount { get; set; }
        public string FileContent { get; set; }
    }
}