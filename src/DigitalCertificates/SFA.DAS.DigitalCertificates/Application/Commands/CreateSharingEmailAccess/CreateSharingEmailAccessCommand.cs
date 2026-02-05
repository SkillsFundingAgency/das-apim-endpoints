using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmailAccess
{
    public class CreateSharingEmailAccessCommand : IRequest<Unit>
    {
        public Guid SharingEmailId { get; set; }
    }
}
