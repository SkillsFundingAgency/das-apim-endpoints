using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.DigitalCertificates.Models;

namespace SFA.DAS.DigitalCertificates.Api.Models.Users
{
    public class GetUserResponse
    {
        public Guid Id { get; set; }
        public string GovUkIdentifier { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsLocked { get; set; }
        

        public static implicit operator GetUserResponse(User source)
        {
            if (source == null) return null;

            return new GetUserResponse
            {
                Id = source.Id,
                GovUkIdentifier = source.GovUkIdentifier,
                EmailAddress = source.EmailAddress,
                PhoneNumber = source.PhoneNumber,
                LastLoginAt = source.LastLoginAt,
                IsLocked = source.IsLocked
            };
        }
    }
}
