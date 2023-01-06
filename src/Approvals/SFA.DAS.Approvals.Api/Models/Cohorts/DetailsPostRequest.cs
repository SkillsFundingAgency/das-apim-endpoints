using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Api.Models.Cohorts
{
    public class DetailsPostRequest
    {
        public CohortSubmissionType SubmissionType { get; set; }
        public string Message { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
