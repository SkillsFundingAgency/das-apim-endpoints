using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class GetVolunteeringOrWorkExperienceItemApiResponse
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string Organisation { get; set; }
    public string Description { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}
