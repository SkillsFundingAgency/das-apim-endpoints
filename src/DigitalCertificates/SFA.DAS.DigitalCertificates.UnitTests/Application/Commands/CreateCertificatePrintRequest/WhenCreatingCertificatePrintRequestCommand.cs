using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificatePrintRequest;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateCertificatePrintRequest
{
    public class WhenCreatingCertificatePrintRequestCommand
    {
        [Test, AutoData]
        public void Then_Command_Properties_Are_Set(
            Guid certificateId,
            Address address,
            EmailData email)
        {
            var command = new CreateCertificatePrintRequestCommand
            {
                CertificateId = certificateId,
                Address = address,
                Email = email
            };

            command.CertificateId.Should().Be(certificateId);
            command.Address.Should().Be(address);
            command.Email.Should().Be(email);
        }

        [Test, AutoData]
        public void Then_Address_Properties_Are_Set(Address address)
        {
            address.ContactName.Should().NotBeNullOrEmpty();
            address.ContactOrganisation.Should().NotBeNullOrEmpty();
            address.ContactAddLine1.Should().NotBeNullOrEmpty();
            address.ContactAddLine2.Should().NotBeNullOrEmpty();
            address.ContactAddLine3.Should().NotBeNullOrEmpty();
            address.ContactAddLine4.Should().NotBeNullOrEmpty();
            address.ContactPostCode.Should().NotBeNullOrEmpty();
        }

        [Test, AutoData]
        public void Then_EmailData_Properties_Are_Set(EmailData email)
        {
            email.EmailAddress.Should().NotBeNullOrEmpty();
            email.UserName.Should().NotBeNullOrEmpty();
            email.LinkDomain.Should().NotBeNullOrEmpty();
            email.TemplateId.Should().NotBeNullOrEmpty();
        }
    }
}
