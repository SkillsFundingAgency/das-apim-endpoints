using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingAccess
{
    public class CreateSharingAccessCommand : IRequest<Unit>
    {
        public Guid SharingId { get; set; }
    }
}
