using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostCreateSharingRequest
    {
        [Test, AutoData]
        public void Then_The_PostUrl_Is_Correctly_Built()
        {
            // Arrange & Act
            var request = new PostCreateSharingRequest(new PostCreateSharingRequestData());

            // Assert
            request.PostUrl.Should().Be("api/sharing");
        }

        [Test, AutoData]
        public void Then_Implicit_Operator_Maps_CreateSharingCommand_Correctly(
            CreateSharingCommand command)
        {
            // Arrange & Act
            PostCreateSharingRequestData requestData = command;

            // Assert
            requestData.UserId.Should().Be(command.UserId);
            requestData.CertificateId.Should().Be(command.CertificateId);
            requestData.CertificateType.Should().Be(command.CertificateType);
            requestData.CourseName.Should().Be(command.CourseName);
        }
    }
}