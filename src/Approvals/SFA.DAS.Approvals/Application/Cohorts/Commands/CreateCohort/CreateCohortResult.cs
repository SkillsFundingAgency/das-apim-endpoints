using System;
using SFA.DAS.Approvals.InnerApi;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.Cohorts.Commands.CreateCohort
{
    public class CreateCohortResult
    {
        public long CohortId { get; set; }
        public string CohortReference { get; set; }
    }
}
