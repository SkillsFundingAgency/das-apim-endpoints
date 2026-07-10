using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.DigitalCertificates.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string GovUkIdentifier { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public List<Name> Names { get; set; } = new List<Name>();

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
                IsLocked = source.IsLocked,
                DateOfBirth = source.DateOfBirth,
                Names = source.Names?.Select(n => new Name
                {
                    UserIdentityId =  n.UserIdentityId,
                    ValidSince = n.ValidSince,
                    ValidUntil = n.ValidUntil,
                    FamilyName = n.FamilyName,
                    GivenNames = n.GivenNames
                }).ToList() ?? new List<Name>()
            };
        }
    }
}
