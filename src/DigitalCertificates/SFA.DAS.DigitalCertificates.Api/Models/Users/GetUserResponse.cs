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
        public DateTime? DateOfBirth { get; set; }
        public List<NameRecord> Names { get; set; } = new List<NameRecord>();

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
                IsLocked = source.IsLocked,
                DateOfBirth = source.DateOfBirth,
                Names = source.Names?.Select(n => new NameRecord
                {
                    UserIdentityId = n.UserIdentityId,
                    ValidSince = n.ValidSince,
                    ValidUntil = n.ValidUntil,
                    FamilyName = n.FamilyName,
                    GivenNames = n.GivenNames
                }).ToList() ?? new List<NameRecord>()
            };
        }
    }

    public class NameRecord
    {
        public Guid UserIdentityId { get; set; }
        public DateTime? ValidSince { get; set; }
        public DateTime? ValidUntil { get; set; }
        public required string FamilyName { get; set; }
        public required string GivenNames { get; set; }
    }
}
