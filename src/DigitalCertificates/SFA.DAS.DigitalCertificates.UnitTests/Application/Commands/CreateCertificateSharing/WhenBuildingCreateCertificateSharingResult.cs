using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificateSharing;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateCertificateSharing
{
    public class WhenBuildingCreateCertificateSharingResult
    {
        [Test, AutoData]
        public void Then_Result_Properties_Are_Set_Correctly(
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
            var result = new CreateCertificateSharingResult
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

            result.UserId.Should().Be(userId);
            result.CertificateId.Should().Be(certificateId);
            result.CertificateType.Should().Be(certificateType);
            result.CourseName.Should().Be(courseName);
            result.SharingId.Should().Be(sharingId);
            result.SharingNumber.Should().Be(sharingNumber);
            result.CreatedAt.Should().Be(createdAt);
            result.LinkCode.Should().Be(linkCode);
            result.ExpiryTime.Should().Be(expiryTime);
        }

        [Test, AutoData]
        public void Then_Implicit_Operator_Maps_PostCreateSharingResponse_Correctly(
            PostCreateSharingResponse response)
        {
            CreateCertificateSharingResult result = response;

            result.UserId.Should().Be(response.UserId);
            result.CertificateId.Should().Be(response.CertificateId);
            result.CertificateType.Should().Be(response.CertificateType);
            result.CourseName.Should().Be(response.CourseName);
            result.SharingId.Should().Be(response.SharingId);
            result.SharingNumber.Should().Be(response.SharingNumber);
            result.CreatedAt.Should().Be(response.CreatedAt);
            result.LinkCode.Should().Be(response.LinkCode);
            result.ExpiryTime.Should().Be(response.ExpiryTime);
        }
    }
}