using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using System;

namespace SFA.DAS.FindAnApprenticeship.Models
{
    public class WorkHistory
    {
        public Guid Id { get; set; }
        public WorkHistoryType WorkHistoryType { get; set; }
        public string Employer { get; set; }
        public string JobTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ApplicationId { get; set; }
        public string Description { get; set; }

        public static implicit operator WorkHistory(GetWorkHistoriesApiResponse source)
        {
            return new WorkHistory
            {
                ApplicationId = source.ApplicationId,
                WorkHistoryType = source.WorkHistoryType,
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
