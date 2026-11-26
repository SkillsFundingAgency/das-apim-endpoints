using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail;
using System;

namespace SFA.DAS.DigitalCertificates.Api.Models.Sharing
{
    public class CreateSharingEmailRequest
    {
        public Guid SharingId { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string LinkDomain { get; set; }
        public string MessageText { get; set; }
        public string TemplateId { get; set; }

        public static implicit operator CreateSharingEmailCommand(CreateSharingEmailRequest source)
        {
            return new CreateSharingEmailCommand
            {
                SharingId = source.SharingId,
                EmailAddress = source.EmailAddress,
                UserName = source.UserName,
                LinkDomain = source.LinkDomain,
                MessageText = source.MessageText,
                TemplateId = source.TemplateId
            };
        }
    }
}
