using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Responses
{
    public class WhenBuildingGetSharingByEmailLinkCodeResponse
    {
        [Test, AutoData]
        public void Then_Response_Can_Be_Constructed(Guid certificateId, string certificateType, DateTime expiryTime, Guid sharingEmailId)
        {
            var response = new GetSharingByEmailLinkCodeResponse
            {
                CertificateId = certificateId,
                CertificateType = certificateType,
                ExpiryTime = expiryTime,
                SharingEmailId = sharingEmailId
            };

            response.CertificateId.Should().Be(certificateId);
            response.CertificateType.Should().Be(certificateType);
            response.ExpiryTime.Should().Be(expiryTime);
            response.SharingEmailId.Should().Be(sharingEmailId);
        }
    }
}
