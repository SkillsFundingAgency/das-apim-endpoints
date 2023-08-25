using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadAddLogResult
    {
        public BulkUploadAddLogResult() { }

        public int LogId { get; set; }

        public static implicit operator BulkUploadAddLogResult(InnerApi.Responses.BulkUploadAddLogResponse response)
        {
            return new BulkUploadAddLogResponse
            {
                LogId = response.LogId
            };
        }
    }
}