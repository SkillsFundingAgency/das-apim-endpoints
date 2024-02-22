using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.GetVolunteering;
public class GetVolunteeringOrWorkExperienceItemQuery : IRequest<GetVolunteeringOrWorkExperienceItemQueryResult>
{
    public Guid Id { get; set; }
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
}
