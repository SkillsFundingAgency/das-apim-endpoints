using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificatePrintRequest;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostCertificatePrintRequest
    {
        [Test, AutoData]
        public void Then_The_PutUrl_Is_Correctly_Built()
        {
            // Arrange & Act
            var request = new PutCertificatePrintRequest(new PutCertificatePrintRequestData(), "123");

            // Assert
            request.PutUrl.Should().Be("api/v1/certificates/123/printrequest");
        }

        [Test, AutoData]
        public void Then_Implicit_Operator_Maps_Command_To_Request_Data(CreateCertificatePrintRequestCommand command)
        {
            // Arrange & Act
            PutCertificatePrintRequestData requestData = command;

            // Assert
            if (command.Address != null)
            {
                requestData.Address.ContactName.Should().Be(command.Address.ContactName);
                requestData.Address.ContactOrganisation.Should().Be(command.Address.ContactOrganisation);
                requestData.Address.ContactAddLine1.Should().Be(command.Address.ContactAddLine1);
                requestData.Address.ContactAddLine2.Should().Be(command.Address.ContactAddLine2);
                requestData.Address.ContactAddLine3.Should().Be(command.Address.ContactAddLine3);
                requestData.Address.ContactAddLine4.Should().Be(command.Address.ContactAddLine4);
                requestData.Address.ContactPostCode.Should().Be(command.Address.ContactPostCode);
            }

            requestData.PrintRequestedBy.Should().Be(command.Address.ContactName);
            requestData.PrintRequestedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }
    }
}
