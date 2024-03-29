﻿using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.GetWorkExperiences
{
    public class GetVolunteeringAndWorkExperiencesQuery : IRequest<GetVolunteeringAndWorkExperiencesQueryResult>
    {
        public Guid CandidateId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
