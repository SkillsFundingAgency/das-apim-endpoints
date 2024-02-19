using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.DeleteVolunteeringOrWorkExperience;
public class GetDeleteVolunteeringOrWorkExperienceQuery : IRequest<GetDeleteVolunteeringOrWorkExperienceQueryResult>
{
    public Guid Id { get; set; }
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
}
