using MediatR;
using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.Cohorts.Commands
{
    public class PostDetailsCommand : IRequest
    {
        public CohortSubmissionType SubmissionType { get; set; }
        public long CohortId { get; set; }
        public string Message { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
