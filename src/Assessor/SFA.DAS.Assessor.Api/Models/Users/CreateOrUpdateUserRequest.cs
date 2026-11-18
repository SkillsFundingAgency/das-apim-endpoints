#nullable enable

using System.ComponentModel.DataAnnotations;
using SFA.DAS.Assessor.Application.Commands.CreateOrUpdateUser;

namespace SFA.DAS.Assessor.Api.Models.Users
{
    public class CreateOrUpdateUserRequest
    {
        [Required]
        [StringLength(255)]
        public required string DfeSignInUserId { get; set; }

        [Required]
        [StringLength(256)]
        public required string EmailAddress { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        public static implicit operator CreateOrUpdateUserCommand(
            CreateOrUpdateUserRequest source)
        {
            return new CreateOrUpdateUserCommand
            {
                DfeSignInUserId = source.DfeSignInUserId,
                EmailAddress = source.EmailAddress,
                PhoneNumber = source.PhoneNumber
            };
        }
    }
}