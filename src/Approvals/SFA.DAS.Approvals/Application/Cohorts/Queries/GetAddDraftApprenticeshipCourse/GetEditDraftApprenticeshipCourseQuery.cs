using MediatR;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries.GetAddDraftApprenticeshipCourse
{
    public class GetAddDraftApprenticeshipCourseQuery : IRequest<GetAddDraftApprenticeshipCourseQueryResult>
    {
        public long AccountLegalEntityId { get; set; }
        public long? ProviderId { get; set; }
    }
}
