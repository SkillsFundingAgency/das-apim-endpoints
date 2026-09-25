using System;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;

namespace SFA.DAS.DigitalCertificates.Models
{
    public class UlnAuthorisation
    {
        public Guid AuthorisationId { get; set; }
        public DateTime AuthorisedAt { get; set; }
        public long Uln { get; set; }

        public static explicit operator UlnAuthorisation(UserAuthorisationResponse source)
        {
            if (source == null) return null;

            return new UlnAuthorisation
            {
                AuthorisationId = source.AuthorisationId,
                AuthorisedAt = source.AuthorisedAt,
                Uln = source.Uln
            };
        }
    }
}
