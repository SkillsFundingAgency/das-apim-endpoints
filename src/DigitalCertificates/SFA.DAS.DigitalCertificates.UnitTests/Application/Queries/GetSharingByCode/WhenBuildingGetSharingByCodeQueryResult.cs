using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByCode;
using SFA.DAS.DigitalCertificates.Models;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetSharingByCode
{
    public class WhenBuildingGetSharingByCodeQueryResult
    {
        [Test, AutoData]
        public void Then_Result_Can_Be_Constructed(Guid certificateId, string certificateType, DateTime expiryTime, Guid sharingId)
        {
            var result = new GetSharingByCodeQueryResult
            {
                Response = new SharingByCode
                {
                    CertificateId = certificateId,
                    CertificateType = certificateType,
                    ExpiryTime = expiryTime,
                    SharingId = sharingId
                },
                BothFound = false
            };

            result.Response.CertificateId.Should().Be(certificateId);
            result.Response.CertificateType.Should().Be(certificateType);
            result.Response.ExpiryTime.Should().Be(expiryTime);
            result.Response.SharingId.Should().Be(sharingId);
            result.BothFound.Should().BeFalse();
        }
    }
}
