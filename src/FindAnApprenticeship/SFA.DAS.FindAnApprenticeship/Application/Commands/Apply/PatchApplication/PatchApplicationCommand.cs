﻿using MediatR;
using SFA.DAS.FindAnApprenticeship.Models;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplication
{
    public class PatchApplicationCommand : IRequest<PatchApplicationCommandResponse>
    {
        public Guid ApplicationId { set; get; }
        public Guid CandidateId { set; get; }
        public SectionStatus WorkExperienceStatus { get; set; }
    }
}
