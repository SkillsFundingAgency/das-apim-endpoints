using System;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.GetVolunteering;
public class GetVolunteeringOrWorkExperienceItemQueryResult
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string Organisation { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public static implicit operator GetVolunteeringOrWorkExperienceItemQueryResult(GetWorkHistoryItemApiResponse source)
    {
        return new GetVolunteeringOrWorkExperienceItemQueryResult
        {
            Id = source.Id,
            ApplicationId = source.ApplicationId,
            Organisation = source.Employer,
            Description = source.Description,
            StartDate = source.StartDate,
            EndDate = source.EndDate
        };
    }
}
