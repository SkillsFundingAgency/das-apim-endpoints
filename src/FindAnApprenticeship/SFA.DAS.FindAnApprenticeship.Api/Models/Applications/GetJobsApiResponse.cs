using System.Collections.Generic;
using System;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class GetJobsApiResponse
    {
        public bool? IsSectionCompleted { get; set; }
        public List<Job> Jobs { get; set; } = [];

        public static implicit operator GetJobsApiResponse(GetJobsQueryResult source)
        {
            return new GetJobsApiResponse
            {
                IsSectionCompleted = source.IsSectionCompleted,
                Jobs = source.Jobs.Select(x => (Job)x).ToList()
            };
        }

        public class Job
        {
            public Guid Id { get; set; }
            public string Employer { get; set; }
            public string JobTitle { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public Guid ApplicationId { get; set; }
            public string Description { get; set; }

            public static implicit operator Job(GetJobsQueryResult.Job source)
            {
                return new Job
                {
                    ApplicationId = source.ApplicationId,
                    Description = source.Description,
                    Employer = source.Employer,
                    EndDate = source.EndDate,
                    Id = source.Id,
                    JobTitle = source.JobTitle,
                    StartDate = source.StartDate
                };
            }
        }
    }
}
