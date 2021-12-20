using System;

namespace SFA.DAS.ApprenticePortal.Models
{
    public class Apprentice
    {
        public Guid ApprenticeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool TermsOfUseAccepted { get; set; }
    }
}
