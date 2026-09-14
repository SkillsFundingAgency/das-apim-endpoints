using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail;
using System;

namespace SFA.DAS.DigitalCertificates.Api.Models.Sharing
{
    public class CreateSharingEmailResponse
    {
        public Guid Id { get; set; }
        public Guid EmailLinkCode { get; set; }

        public static implicit operator CreateSharingEmailResponse(CreateSharingEmailResult source)
        {
            if (source == null) return null;
            return new CreateSharingEmailResponse
            {
                Id = source.Id,
                EmailLinkCode = source.EmailLinkCode
            };
        }
    }
}
