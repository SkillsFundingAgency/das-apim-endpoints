using System;

namespace SFA.DAS.DigitalCertificates.Models
{
    public class UlnAuthorisation
    {
        public Guid AuthorisationId { get; set; }
        public DateTime AuthorisedAt { get; set; }
        public long Uln { get; set; }
    }
}
