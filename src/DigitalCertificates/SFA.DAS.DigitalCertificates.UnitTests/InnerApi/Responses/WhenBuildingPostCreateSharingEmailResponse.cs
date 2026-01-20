using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Responses
{
    public class WhenBuildingPostCreateSharingEmailResponse
    {
        [Test, AutoData]
        public void Then_Response_Can_Be_Constructed(
            Guid id,
            Guid emailLinkCode)
        {
            // Arrange & Act
            var response = new PostCreateSharingEmailResponse
            {
                Id = id,
                EmailLinkCode = emailLinkCode,
            };

            // Assert
            response.Id.Should().Be(id);
            response.EmailLinkCode.Should().Be(emailLinkCode);
            
        }
    }
}
