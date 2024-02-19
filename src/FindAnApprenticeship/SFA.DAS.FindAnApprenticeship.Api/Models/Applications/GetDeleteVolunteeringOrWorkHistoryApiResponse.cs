using System;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.DeleteVolunteeringOrWorkExperience;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetDeleteVolunteeringOrWorkHistoryApiResponse
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string Organisation { get; set; }
    public string Description { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    public static implicit operator GetDeleteVolunteeringOrWorkHistoryApiResponse(GetDeleteVolunteeringOrWorkExperienceQueryResult source)
    {
        return new GetDeleteVolunteeringOrWorkHistoryApiResponse
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
