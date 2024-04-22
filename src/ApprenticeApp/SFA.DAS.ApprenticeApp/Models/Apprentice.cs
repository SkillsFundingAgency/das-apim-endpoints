using System;

namespace SFA.DAS.ApprenticeApp.Models
{
    public class Apprentice
    {
        public Guid ApprenticeId { get; set; }
        public string FirstName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool TermsOfUseAccepted { get; set; }
    }
}
