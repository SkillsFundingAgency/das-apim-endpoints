using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class RegistrationsRemindersInvitationsResponse
    {
        public IEnumerable<Registration> Registrations { get; set; }

        public class Registration
        {
            public Guid RegistrationId { get; set; }
            public long ApprenticeshipId { get; set; }
            public string Email { get; set; }
            public string EmployerName { get; set; }
        }
    }
}
