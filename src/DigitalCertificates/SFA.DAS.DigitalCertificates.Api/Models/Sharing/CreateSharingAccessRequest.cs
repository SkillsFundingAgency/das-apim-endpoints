using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingAccess;
using System;

namespace SFA.DAS.DigitalCertificates.Api.Models.Sharing
{
    public class CreateSharingAccessRequest
    {
        public Guid SharingId { get; set; }

        public static implicit operator CreateSharingAccessCommand(CreateSharingAccessRequest source)
        {
            return new CreateSharingAccessCommand
            {
                SharingId = source.SharingId,
            };
        }
    }
}
