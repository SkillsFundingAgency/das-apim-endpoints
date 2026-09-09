using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Responses
{
    public class WhenBuildingGetSharingByLinkCodeResponse
    {
        [Test, AutoData]
        public void Then_Response_Can_Be_Constructed(Guid certificateId, string certificateType, DateTime expiryTime, Guid sharingId)
        {
            var response = new GetSharingByLinkCodeResponse
            {
                CertificateId = certificateId,
                CertificateType = certificateType,
                ExpiryTime = expiryTime,
                SharingId = sharingId
            };

            response.CertificateId.Should().Be(certificateId);
            response.CertificateType.Should().Be(certificateType);
            response.ExpiryTime.Should().Be(expiryTime);
            response.SharingId.Should().Be(sharingId);
        }
    }
}
