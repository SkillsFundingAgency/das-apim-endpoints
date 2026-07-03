using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.Models
{
    public class UpdateUserIdentityRequest
    {
        public required List<Name> Names { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
