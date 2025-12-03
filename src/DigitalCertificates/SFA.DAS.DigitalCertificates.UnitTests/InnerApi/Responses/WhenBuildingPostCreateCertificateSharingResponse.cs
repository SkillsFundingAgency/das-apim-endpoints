using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Responses
{
    public class WhenBuildingPostCreateCertificateSharingResponse
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
            DateTime expiryTime)
        {

            var response = new PostCreateSharingResponse
            {
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = certificateType,
                CourseName = courseName,
                SharingId = sharingId,
                SharingNumber = sharingNumber,
                CreatedAt = createdAt,
                LinkCode = linkCode,
                ExpiryTime = expiryTime
            };

            response.UserId.Should().Be(userId);
            response.CertificateId.Should().Be(certificateId);
            response.CertificateType.Should().Be(certificateType);
            response.CourseName.Should().Be(courseName);
            response.SharingId.Should().Be(sharingId);
            response.SharingNumber.Should().Be(sharingNumber);
            response.CreatedAt.Should().Be(createdAt);
            response.LinkCode.Should().Be(linkCode);
            response.ExpiryTime.Should().Be(expiryTime);
        }
    }
}