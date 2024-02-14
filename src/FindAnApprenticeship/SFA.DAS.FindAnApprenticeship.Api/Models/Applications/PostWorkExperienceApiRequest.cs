using System;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateWorkExperience;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class PostWorkExperienceApiRequest
    {
        public Guid CandidateId { get; set; }
        public string EmployerName { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class PostWorkExperienceApiResponse
    {
        public Guid Id { get; set; }

        public static implicit operator PostWorkExperienceApiResponse(CreateWorkCommandResponse source)
        {
            return new PostWorkExperienceApiResponse
            {
                Id = source.Id
            };
        }
    }
}
