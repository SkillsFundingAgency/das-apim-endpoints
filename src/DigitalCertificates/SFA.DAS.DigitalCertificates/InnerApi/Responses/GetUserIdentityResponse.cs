using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.InnerApi.Responses
{
    public class GetUserIdentityResponse
    {
        public List<IdentityName> Identity { get; set; } = [];
        public DateTime? DateOfBirth { get; set; }
        public IdentityAuthorisation Authorisation { get; set; }
        public List<long> Excluded { get; set; } = [];
    }

    public class IdentityName
    {
        public DateTime? ValidFrom { get; set; }
        public string FamilyName { get; set; }
        public string GivenNames { get; set; }
    }

    public class IdentityAuthorisation
    {
        public Guid AuthorisationId { get; set; }
        public DateTime AuthorisedAt { get; set; }
        public long Uln { get; set; }
    }
}
