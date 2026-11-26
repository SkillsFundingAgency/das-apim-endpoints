using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail
{
    public class CreateSharingEmailCommand : IRequest<CreateSharingEmailResult>
    {
        public Guid SharingId { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string LinkDomain { get; set; }
        public string MessageText { get; set; }
        public string TemplateId { get; set; }
    }
}
