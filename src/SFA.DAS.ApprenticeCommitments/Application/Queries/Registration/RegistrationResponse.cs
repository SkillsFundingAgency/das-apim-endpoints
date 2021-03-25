using System;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.Registration
{
    public class RegistrationResponse
    {
        public Guid ApprenticeId { get; set; }
        public string Email { get; set; }
    }
}
