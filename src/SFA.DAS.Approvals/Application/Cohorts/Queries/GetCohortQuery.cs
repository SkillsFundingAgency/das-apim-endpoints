using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

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
