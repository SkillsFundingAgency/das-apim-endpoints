using System;
using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorise
{
    public class CreateUserAuthoriseCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public long Uln { get; set; }
    }
}
