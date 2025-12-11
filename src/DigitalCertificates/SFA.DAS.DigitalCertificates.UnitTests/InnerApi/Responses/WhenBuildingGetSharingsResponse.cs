using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using static SFA.DAS.DigitalCertificates.Models.Enums;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Responses
{
    public class WhenBuildingGetSharingsResponse
    {
        [Test, AutoData]
        public void Then_Sharing_Items_Can_Be_Constructed()
        {
            // Arrange
            var sharing = new SharingItem
            {
                SharingId = Guid.NewGuid(),
                SharingNumber = 1,
                CreatedAt = DateTime.UtcNow,
                LinkCode = Guid.NewGuid(),
                ExpiryTime = DateTime.UtcNow.AddDays(1),
                SharingAccess = new List<DateTime> { DateTime.UtcNow },
                SharingEmails = new List<SharingEmailItem>
                {
                    new SharingEmailItem
                    {
                        SharingEmailId = Guid.NewGuid(),
                        EmailAddress = "test@example.com",
                        EmailLinkCode = Guid.NewGuid(),
                        SentTime = DateTime.UtcNow,
                        SharingEmailAccess = new List<DateTime> { DateTime.UtcNow }
                    }
                }
            };

            // Act
            var response = new GetSharingsResponse
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = "CourseName",
                Sharings = new List<SharingItem> { sharing }
            };

            // Assert
            response.Sharings.Should().ContainSingle();
            response.Sharings[0].SharingEmails.Should().ContainSingle();
            response.CertificateType.Should().Be(CertificateType.Standard);
        }
    }
}
