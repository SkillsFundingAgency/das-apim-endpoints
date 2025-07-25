using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.DigitalCertificates.InnerApi.Responses
{
    [ExcludeFromCodeCoverage]
    public class User
    {
        public Guid Id { get; set; }
        public string GovUkIdentifier {get; set;}
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime? LockedAt { get; set; }
    }
}
