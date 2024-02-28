using MediatR;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries
{
    public class GetCohortQuery : IRequest<GetCohortResult>
    {
        public GetCohortQuery(long cohortId)
        {
            CohortId = cohortId;
        }

        public long CohortId { get; set; }
    }
}
