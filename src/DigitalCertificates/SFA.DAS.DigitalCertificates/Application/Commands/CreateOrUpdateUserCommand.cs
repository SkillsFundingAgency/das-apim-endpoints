using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.DigitalCertificates.Models;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser
{
    public class CreateOrUpdateUserCommand : IRequest<CreateOrUpdateUserResult>
    {
        public required string GovUkIdentifier { get; set; }
        public required string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }

        public required List<Name> Names { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
