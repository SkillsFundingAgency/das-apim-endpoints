using System;
using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Commands.DeleteSharing
{
    public class DeleteSharingCommand : IRequest
    {
        public Guid SharingId { get; set; }
    }
}
