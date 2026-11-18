#nullable enable

using System;
using SFA.DAS.Assessor.Models;

namespace SFA.DAS.Assessor.Api.Models.Users
{
    public class GetUserResponse
    {
        public Guid Id { get; set; }
        public string? DfeSignInUserId { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsLocked { get; set; }

        public static implicit operator GetUserResponse(User source)
        {
            return new GetUserResponse
            {
                Id = source.Id,
                DfeSignInUserId = source.DfeSignInUserId,
                EmailAddress = source.EmailAddress,
                PhoneNumber = source.PhoneNumber,
                LastLoginAt = source.LastLoginAt,
                CreatedAt = source.CreatedAt,
                IsLocked = source.IsLocked
            };
        }
    }
}