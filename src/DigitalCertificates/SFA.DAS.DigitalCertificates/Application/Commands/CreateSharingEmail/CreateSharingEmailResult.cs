using System;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail
{
    public class CreateSharingEmailResult
    {
        public Guid Id { get; set; }
        public Guid EmailLinkCode { get; set; }
        public static implicit operator CreateSharingEmailResult(PostCreateSharingEmailResponse response)
        {
            return new CreateSharingEmailResult
            {
                Id = response.Id,
                EmailLinkCode = response.EmailLinkCode,
            };
        }
    }
}
