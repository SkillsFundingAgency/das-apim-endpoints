using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.ApprenticeAccountsApi
{
    public class Apprentice
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Email Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool TermsOfUseAccepted { get; set; }

    }

    public class Email 
    {
        public string DisplayName { get; set; }
        public string User { get; set; }
        public string Host { get; set; }
        public string Address { get; set; }
    }
}