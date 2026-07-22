using System;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorise;

namespace SFA.DAS.DigitalCertificates.Api.Models.Users
{
    public class CreateUserAuthoriseRequest
    {
        public long Uln { get; set; }

        public static implicit operator CreateUserAuthoriseCommand(CreateUserAuthoriseRequest source)
        {
            return new CreateUserAuthoriseCommand
            {
                Uln = source.Uln
            };
        }
    }
}
