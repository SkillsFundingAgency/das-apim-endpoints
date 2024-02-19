using System;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.DeleteVolunteeringOrWorkExperience;
public class GetDeleteVolunteeringOrWorkExperienceQueryResult
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string Organisation { get; set; }
    public string Description { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    public static implicit operator GetDeleteVolunteeringOrWorkExperienceQueryResult(GetVolunteeringOrWorkExperienceItemApiResponse source)
    {
        return new GetDeleteVolunteeringOrWorkExperienceQueryResult
        {
            Id = source.Id,
            ApplicationId = source.ApplicationId,
            Organisation = source.Organisation,
            Description = source.Description,
            FromDate = source.FromDate,
            ToDate = source.ToDate
        };
    }
}
