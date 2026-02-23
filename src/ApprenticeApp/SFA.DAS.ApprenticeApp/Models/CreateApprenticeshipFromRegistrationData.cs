using System;

namespace SFA.DAS.ApprenticeApp.Models
{
    public class CreateApprenticeshipFromRegistrationData
    {
        public Guid RegistrationId { get; set; }
        public Guid ApprenticeId { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
