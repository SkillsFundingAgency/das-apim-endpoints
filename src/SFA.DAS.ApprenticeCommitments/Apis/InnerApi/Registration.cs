using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class Registration
    {
        public Guid RegistrationId { get; set; }
        public long ApprenticeshipId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string EmployerName { get; set; }
        public string CourseName { get; set; }
    }

    public class RegistrationsWrapper
    {
        public IEnumerable<Registration> Registrations { get; set; }
    }
}