﻿using System;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipResponse
    {
        public Guid SourceId { get; set; }
        public string Email { get; internal set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string ApprenticeshipName { get; set; }
    }
}