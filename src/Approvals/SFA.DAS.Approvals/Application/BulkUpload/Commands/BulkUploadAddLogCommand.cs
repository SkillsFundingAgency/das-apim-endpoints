using MediatR;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadAddLogCommand : IRequest<BulkUploadAddLogResult>
    {
        public BulkUploadAddLogCommand()
        {
        }

        public long ProviderId { get; set; }
        public string FileName { get; set; }
        public int? RplCount { get; set; }
        public int? RowCount { get; set; }
        public string FileContent { get; set; }
    }
}