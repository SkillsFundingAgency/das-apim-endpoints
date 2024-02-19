using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory
{
    public class GetJobsQueryResult
    {
        public List<Job> Jobs { get; set; } = [];

        public static implicit operator GetJobsQueryResult(GetWorkHistoriesApiResponse source)
        {
            return new GetJobsQueryResult
            {
                Jobs = source.WorkHistories.Select(x => (Job) x).ToList()
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

            public static implicit operator Job(GetWorkHistoriesApiResponse.WorkHistoryItem source)
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
