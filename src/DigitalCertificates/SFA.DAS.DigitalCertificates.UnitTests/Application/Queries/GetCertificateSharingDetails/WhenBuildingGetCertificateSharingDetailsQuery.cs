using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharings;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetCertificateSharingDetails
{
    public class WhenBuildingGetCertificateSharingDetailsQuery
    {
        [Test, AutoData]
        public void Then_Properties_Can_Be_Set(Guid userId, Guid certificateId, int limit)
        {
            var query = new GetCertificateSharingDetailsQuery
            {
                UserId = userId,
                CertificateId = certificateId,
                Limit = limit
            };

            query.UserId.Should().Be(userId);
            query.CertificateId.Should().Be(certificateId);
            query.Limit.Should().Be(limit);
        }
    }
}
