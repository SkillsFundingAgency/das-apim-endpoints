using System;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateJob;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class PostJobApiRequest
    {
        public Guid CandidateId { get; set; }
        public string EmployerName { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class PostJobApiResponse
    {
        public Guid Id { get; set; }

        public static implicit operator PostJobApiResponse(CreateJobCommandResponse source)
        {
            return new PostJobApiResponse
            {
                Id = source.Id
            };
        }
    }
}
