using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetWorkExperiences;

public record GetVolunteeringAndWorkExperiencesQueryResult
{
    public List<VolunteeringAndWorkExperience> VolunteeringAndWorkExperiences { get; set; } = [];

    public static implicit operator GetVolunteeringAndWorkExperiencesQueryResult(GetWorkHistoriesApiResponse source)
    {
        return new GetVolunteeringAndWorkExperiencesQueryResult
        {
            VolunteeringAndWorkExperiences = source.WorkHistories.Select(x => (VolunteeringAndWorkExperience)x).ToList()
        };
    }

    public record VolunteeringAndWorkExperience
    {
        public Guid Id { get; set; }
        public string Employer { get; set; }
        public string JobTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ApplicationId { get; set; }
        public string Description { get; set; }

        public static implicit operator VolunteeringAndWorkExperience(GetWorkHistoriesApiResponse.WorkHistoryItem source)
        {
            return new VolunteeringAndWorkExperience
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