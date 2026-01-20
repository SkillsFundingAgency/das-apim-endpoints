using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateSharingEmail
{
    public class WhenBuildingCreateSharingEmailCommand
    {
        [Test]
        public void Then_Command_Properties_Are_Set_Correctly()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var emailAddress = "test@example.com";
            var userName = "User Name";
            var linkDomain = "https://example.com";
            var templateId = Guid.NewGuid().ToString();

            // Act
            var command = new CreateSharingEmailCommand
            {
                SharingId = sharingId,
                EmailAddress = emailAddress,
                UserName = userName,
                LinkDomain = linkDomain,
                TemplateId = templateId
            };

            // Assert
            command.SharingId.Should().Be(sharingId);
            command.EmailAddress.Should().Be(emailAddress);
            command.UserName.Should().Be(userName);
            command.LinkDomain.Should().Be(linkDomain);
            command.TemplateId.Should().Be(templateId);
        }
    }
}
