using System;

namespace SFA.DAS.DigitalCertificates.Models
{
    public class Name
    {
        public DateTime? ValidSince { get; set; }
        public DateTime? ValidUntil { get; set; }
        public required string FamilyName { get; set; }
        public required string GivenNames { get; set; }

    }
}
