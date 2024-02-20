using System;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.GetVolunteeringOrWorkExperienceItem;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.DeleteVolunteeringOrWorkExperience;
public class GetVolunteeringOrWorkExperienceItemQuery : IRequest<GetVolunteeringOrWorkExperienceItemQueryResult>
{
    public Guid Id { get; set; }
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
}
