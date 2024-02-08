using System;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory.DeleteJob;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class GetDeleteJobApiResponse
    {
        public Guid Id { get; set; }
        public string Employer { get; set; }
        public string JobTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ApplicationId { get; set; }
        public string Description { get; set; }

        public static implicit operator GetDeleteJobApiResponse(GetDeleteJobQueryResult source)
        {
            return new GetDeleteJobQueryResult
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
