using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmailAccess;
using System;

namespace SFA.DAS.DigitalCertificates.Api.Models.Sharing
{
    public class CreateSharingEmailAccessRequest
    {
        public Guid SharingEmailId { get; set; }

        public static implicit operator CreateSharingEmailAccessCommand(CreateSharingEmailAccessRequest source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return new CreateSharingEmailAccessCommand
            {
                SharingEmailId = source.SharingEmailId,
            };
        }
    }
}
