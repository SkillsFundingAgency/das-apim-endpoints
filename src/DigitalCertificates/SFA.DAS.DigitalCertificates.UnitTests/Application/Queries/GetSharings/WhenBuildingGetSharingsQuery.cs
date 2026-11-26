using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharings;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetSharings
{
    public class WhenBuildingGetSharingsQuery
    {
        [Test, AutoData]
        public void Then_Properties_Can_Be_Set(Guid userId, Guid certificateId, int limit)
        {
            // Arrange & Act
            var query = new GetSharingsQuery
            {
                UserId = userId,
                CertificateId = certificateId,
                Limit = limit
            };

            // Assert
            query.UserId.Should().Be(userId);
            query.CertificateId.Should().Be(certificateId);
            query.Limit.Should().Be(limit);
        }
    }
}
