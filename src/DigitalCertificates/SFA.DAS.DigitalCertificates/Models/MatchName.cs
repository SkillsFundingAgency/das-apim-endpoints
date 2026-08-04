using System;

namespace SFA.DAS.DigitalCertificates.Models
{
    public class MatchName
    {
        public Guid UserIdentityId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FamilyName { get; set; }
    }
}
