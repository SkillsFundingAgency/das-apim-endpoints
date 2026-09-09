using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Models;

namespace SFA.DAS.DigitalCertificates.UnitTests.Models
{
    public class WhenBuildingSharingByCode
    {
        [Test, AutoData]
        public void Then_Model_Can_Be_Constructed(Guid certificateId, string certificateType, DateTime expiryTime, Guid sharingId, Guid sharingEmailId)
        {
            var model = new SharingByCode
            {
                CertificateId = certificateId,
                CertificateType = certificateType,
                ExpiryTime = expiryTime,
                SharingId = sharingId,
                SharingEmailId = sharingEmailId
            };

            model.CertificateId.Should().Be(certificateId);
            model.CertificateType.Should().Be(certificateType);
            model.ExpiryTime.Should().Be(expiryTime);
            model.SharingId.Should().Be(sharingId);
            model.SharingEmailId.Should().Be(sharingEmailId);
        }
    }
}
