using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory
{
    public class GetJobsQueryResult
    {
        public List<Job> Jobs { get; set; } = [];
        public bool? IsSectionCompleted { get; set; }
       

        public class Job
        {
            public Guid Id { get; set; }
            public string Employer { get; set; }
            public string JobTitle { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public Guid ApplicationId { get; set; }
            public string Description { get; set; }
        }
    }
}
