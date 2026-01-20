using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateSharingEmail
{
    public class WhenBuildingCreateSharingEmailResult
    {
        [Test, AutoData]
        public void Then_Result_Properties_Are_Set_Correctly(
            Guid id,
            Guid emailLinkCode,
            DateTime expiryTime)
        {
            // Arrange & Act
            var result = new CreateSharingEmailResult
            {
                Id = id,
                EmailLinkCode = emailLinkCode,
            };

            // Assert
            result.Id.Should().Be(id);
            result.EmailLinkCode.Should().Be(emailLinkCode);
        }

        [Test, AutoData]
        public void Then_Implicit_Operator_Maps_PostCreateSharingEmailResponse_Correctly(
            PostCreateSharingEmailResponse response)
        {
            // Arrange & Act
            CreateSharingEmailResult result = response;

            // Assert
            result.Id.Should().Be(response.Id);
            result.EmailLinkCode.Should().Be(response.EmailLinkCode);
        }
    }
}
