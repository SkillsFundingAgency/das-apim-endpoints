using System;

namespace SFA.DAS.DigitalCertificates.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string GovUkIdentifier { get; set; }
        public string EmailAddress { get; set;  }
        public string PhoneNumber { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime? LockedAt { get; set; }

        public static explicit operator User(InnerApi.Responses.User source)
        {
            if (source == null) return null;

            return new User()
            {
                Id = source.Id,
                GovUkIdentifier = source.GovUkIdentifier,
                EmailAddress = source.EmailAddress,
                PhoneNumber = source.PhoneNumber,
                LastLoginAt = source.LastLoginAt,
                LockedAt = source.LockedAt
            };
        }
    }
}
