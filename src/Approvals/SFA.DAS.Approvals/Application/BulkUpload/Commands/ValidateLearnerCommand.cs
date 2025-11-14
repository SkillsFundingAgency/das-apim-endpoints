using MediatR;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class ValidateLearnerCommand : IRequest<Unit>
    {
        public long ProviderId { get; set; }
        public long LearnerDataId { get; set; }
    }
}
