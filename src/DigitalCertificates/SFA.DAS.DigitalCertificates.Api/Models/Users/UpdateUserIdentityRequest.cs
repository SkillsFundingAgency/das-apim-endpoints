using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.DigitalCertificates.Application.Commands.UpdateUserIdentity;
using SFA.DAS.DigitalCertificates.Models;

namespace SFA.DAS.DigitalCertificates.Api.Models.Users
{
    public class UpdateUserIdentityRequest
    {
        public List<UpdateNameRecord> Names { get; set; } = new List<UpdateNameRecord>();
        public DateTime DateOfBirth { get; set; }

        public static implicit operator UpdateUserIdentityCommand(UpdateUserIdentityRequest source)
        {
            return new UpdateUserIdentityCommand
            {
                Names = source.Names?.Select(n => new Name
                {
                    UserIdentityId = n.UserIdentityId,
                    ValidSince = n.ValidSince,
                    ValidUntil = n.ValidUntil,
                    FamilyName = n.FamilyName,
                    GivenNames = n.GivenNames
                }).ToList() ?? new List<Name>(),
                DateOfBirth = source.DateOfBirth
            };
        }
    }

    public class UpdateNameRecord
    {
        public Guid UserIdentityId { get; set; }
        public DateTime? ValidSince { get; set; }
        public DateTime? ValidUntil { get; set; }
        public required string FamilyName { get; set; }
        public required string GivenNames { get; set; }
    }
}
