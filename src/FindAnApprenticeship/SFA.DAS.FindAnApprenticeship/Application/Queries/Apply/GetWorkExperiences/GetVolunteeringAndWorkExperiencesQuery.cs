using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetWorkExperiences
{
    public class GetVolunteeringAndWorkExperiencesQuery : IRequest<GetVolunteeringAndWorkExperiencesQueryResult>
    {
        public Guid CandidateId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
