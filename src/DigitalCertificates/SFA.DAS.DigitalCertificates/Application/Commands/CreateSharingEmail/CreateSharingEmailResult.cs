using System;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail
{
    public class CreateSharingEmailResult
    {
        public Guid Id { get; set; }
        public Guid EmailLinkCode { get; set; }
        public static implicit operator CreateSharingEmailResult(CreateSharingEmailResponse response)
        {
            return new CreateSharingEmailResult
            {
                Id = response.Id,
                EmailLinkCode = response.EmailLinkCode,
            };
        }
    }
}
