using System;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.GetVolunteering;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetVolunteeringOrWorkExperienceItemApiResponse
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string Organisation { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public static implicit operator GetVolunteeringOrWorkExperienceItemApiResponse(GetVolunteeringOrWorkExperienceItemQueryResult source)
    {
        return new GetVolunteeringOrWorkExperienceItemApiResponse
        {
            Id = source.Id,
            ApplicationId = source.ApplicationId,
            Organisation = source.Organisation,
            Description = source.Description,
            StartDate = source.StartDate,
            EndDate = source.EndDate
        };
    }
}
