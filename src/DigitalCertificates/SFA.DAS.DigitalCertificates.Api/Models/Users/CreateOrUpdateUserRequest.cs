using System;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser;

namespace SFA.DAS.DigitalCertificates.Api.Models.Users
{
    public class CreateOrUpdateUserRequest
    {
        public required string GovUkIdentifier { get; set; }
        public required string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        public static implicit operator CreateOrUpdateUserCommand(CreateOrUpdateUserRequest source)
        {
            return new CreateOrUpdateUserCommand
            {
                GovUkIdentifier = source.GovUkIdentifier,
                EmailAddress = source.EmailAddress,
                PhoneNumber = source.PhoneNumber
            };
        }
    }
}
