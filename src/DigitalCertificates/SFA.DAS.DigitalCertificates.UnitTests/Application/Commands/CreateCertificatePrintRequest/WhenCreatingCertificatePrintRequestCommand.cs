using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificatePrintRequest;
using SFA.DAS.DigitalCertificates.Constants;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateCertificatePrintRequest
{
    public class WhenCreatingCertificatePrintRequestCommand
    {
        [Test, AutoData]
        public void Then_CertificateId_Is_Set(Guid certificateId)
        {
            var command = new CreateCertificatePrintRequestCommand { CertificateId = certificateId };

            command.CertificateId.Should().Be(certificateId);
        }

        [Test, AutoData]
        public void Then_Address_Properties_Are_Mapped_To_RequestData(CreateCertificatePrintRequestCommand command)
        {
            // Act
            PutCertificatePrintRequestData requestData = command;

            // Assert
            requestData.Address.ContactName.Should().Be(command.Address.ContactName);
            requestData.Address.ContactOrganisation.Should().Be(command.Address.ContactOrganisation);
            requestData.Address.ContactAddLine1.Should().Be(command.Address.ContactAddLine1);
            requestData.Address.ContactAddLine2.Should().Be(command.Address.ContactAddLine2);
            requestData.Address.ContactAddLine3.Should().Be(command.Address.ContactAddLine3);
            requestData.Address.ContactAddLine4.Should().Be(command.Address.ContactAddLine4);
            requestData.Address.ContactPostCode.Should().Be(command.Address.ContactPostCode);
        }

        [Test]
        public void Then_Null_Address_Maps_To_Null_RequestData_Address()
        {
            var command = new CreateCertificatePrintRequestCommand { Address = null };

            PutCertificatePrintRequestData requestData = command;

            requestData.Address.Should().BeNull();
        }

        [Test, AutoData]
        public void Then_PrintRequestedBy_Is_Set_To_Constant(CreateCertificatePrintRequestCommand command)
        {
            PutCertificatePrintRequestData requestData = command;

            requestData.PrintRequestedBy.Should().Be(CertificateConstants.PrintRequestedBy);
        }

        [Test, AutoData]
        public void Then_PrintRequestedAt_Is_Set_To_UtcNow(CreateCertificatePrintRequestCommand command)
        {
            PutCertificatePrintRequestData requestData = command;

            requestData.PrintRequestedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }
    }
}
