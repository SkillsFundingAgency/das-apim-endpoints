﻿using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteWorkExperience;
public class PostDeleteVolunteeringOrWorkExperienceCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
}
