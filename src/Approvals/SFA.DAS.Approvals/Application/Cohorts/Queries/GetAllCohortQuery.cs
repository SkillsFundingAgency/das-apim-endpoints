using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries
{
    public class GetAllCohortQuery : IRequest<GetCohortResult>
    {
        public GetAllCohortQuery(long cohortId)
        {
            CohortId = cohortId;
        }

        public long CohortId { get; set; }
    }
}
