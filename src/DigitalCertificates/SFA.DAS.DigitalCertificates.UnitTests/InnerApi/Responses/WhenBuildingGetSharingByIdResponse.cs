using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.DigitalCertificates.Models;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Responses
{
    public class WhenBuildingGetSharingByIdResponse
    {
        [Test, AutoData]
        public void Then_Response_Can_Be_Constructed(
        Guid userId,
        Guid certificateId,
        string certificateType,
        string courseName,
        Guid sharingId,
        int sharingNumber,
        DateTime createdAt,
        Guid linkCode,
        DateTime expiryTime,
        List<DateTime> sharingAccess,
        List<SharingEmail> sharingEmails)
        {
            // Arrange & Act
            var response = new GetSharingByIdResponse
            {
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = certificateType,
                CourseName = courseName,
                SharingId = sharingId,
                SharingNumber = sharingNumber,
                CreatedAt = createdAt,
                LinkCode = linkCode,
                ExpiryTime = expiryTime,
                SharingAccess = sharingAccess,
                SharingEmails = sharingEmails
            };

            // Assert
            response.UserId.Should().Be(userId);
            response.CertificateId.Should().Be(certificateId);
            response.CertificateType.Should().Be(certificateType);
            response.CourseName.Should().Be(courseName);
            response.SharingId.Should().Be(sharingId);
            response.SharingNumber.Should().Be(sharingNumber);
            response.CreatedAt.Should().Be(createdAt);
            response.LinkCode.Should().Be(linkCode);
            response.ExpiryTime.Should().Be(expiryTime);
            response.SharingAccess.Should().BeEquivalentTo(sharingAccess);
            response.SharingEmails.Should().BeEquivalentTo(sharingEmails);
        }
    }
}
