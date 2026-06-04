using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificatePrintRequest
{
    public class Address
    {
        public string ContactName { get; set; }
        public string ContactOrganisation { get; set; }
        public string ContactAddLine1 { get; set; }
        public string ContactAddLine2 { get; set; }
        public string ContactAddLine3 { get; set; }
        public string ContactAddLine4 { get; set; }
        public string ContactPostCode { get; set; }
    }

    public class EmailData
    {
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string LinkDomain { get; set; }
        public string TemplateId { get; set; }
    }

    public class CreateCertificatePrintRequestCommand : IRequest<Unit>
    {
        public Guid CertificateId { get; set; }
        public Address Address { get; set; }
        public EmailData Email { get; set; }
    }
}
