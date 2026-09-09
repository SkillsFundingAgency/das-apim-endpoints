using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostCreateSharingEmailRequest
    {
        [Test, AutoData]
        public void Then_The_PostUrl_Is_Correctly_Built()
        {
            // Arrange & Act
            var request = new PostCreateSharingEmailRequest(new PostCreateSharingEmailRequestData(), "123");

            // Assert
            request.PostUrl.Should().Be("api/sharing/123/email");
        }

        [Test, AutoData]
        public void Then_Implicit_Operator_Maps_CreateSharingEmailCommand_Correctly(
            CreateSharingEmailCommand command)
        {
            // Arrange & Act
            PostCreateSharingEmailRequestData requestData = command;

            // Assert
            requestData.EmailAddress.Should().Be(command.EmailAddress);
        }
    }
}
