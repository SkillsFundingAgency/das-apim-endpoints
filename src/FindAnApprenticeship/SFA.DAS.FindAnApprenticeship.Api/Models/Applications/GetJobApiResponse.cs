using System;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetJob;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class GetJobApiResponse
    {
        public Guid Id { get; set; }
        public string Employer { get; set; }
        public string JobTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ApplicationId { get; set; }
        public string Description { get; set; }

        public static implicit operator GetJobApiResponse(GetJobQueryResult source)
        {
            return new GetJobApiResponse
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
