using MediatR;
using SFA.DAS.DigitalCertificates.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser
{
    public class CreateOrUpdateUserCommand : IRequest<CreateOrUpdateUserResponse>
    {
        public required string GovUkIdentifier { get; set; }
        public required string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }

        public required List<Name> Names { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
