using SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificatePrintRequest;
using System;

namespace SFA.DAS.DigitalCertificates.Api.Models.Certificates
{
    public class CreatePrintRequest
    {
        public Guid CertificateId { get; set; }
        public AddressDto Address { get; set; }
        public EmailDto Email { get; set; }

        public class AddressDto
        {
            public string ContactName { get; set; }
            public string ContactOrganisation { get; set; }
            public string ContactAddLine1 { get; set; }
            public string ContactAddLine2 { get; set; }
            public string ContactAddLine3 { get; set; }
            public string ContactAddLine4 { get; set; }
            public string ContactPostCode { get; set; }
        }

        public class EmailDto
        {
            public string EmailAddress { get; set; }
            public string UserName { get; set; }
            public string LinkDomain { get; set; }
            public string TemplateId { get; set; }
        }

        public static implicit operator CreateCertificatePrintRequestCommand(CreatePrintRequest source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return new CreateCertificatePrintRequestCommand
            {
                CertificateId = source.CertificateId,
                Address = source.Address == null ? null : new Address
                {
                    ContactName = source.Address.ContactName,
                    ContactOrganisation = source.Address.ContactOrganisation,
                    ContactAddLine1 = source.Address.ContactAddLine1,
                    ContactAddLine2 = source.Address.ContactAddLine2,
                    ContactAddLine3 = source.Address.ContactAddLine3,
                    ContactAddLine4 = source.Address.ContactAddLine4,
                    ContactPostCode = source.Address.ContactPostCode
                },
                Email = source.Email == null ? null : new EmailData
                {
                    EmailAddress = source.Email.EmailAddress,
                    UserName = source.Email.UserName,
                    LinkDomain = source.Email.LinkDomain,
                    TemplateId = source.Email.TemplateId
                }
            };
        }
    }
}
