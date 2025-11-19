using MediatR;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class ValidateLearnerCommand : IRequest<LearnerValidateApiResponse>
    {
        public long ProviderId { get; set; }
        public long LearnerDataId { get; set; }
    }
}
