using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharedStandardCertificate;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetSharedStandardCertificate
{
    public class WhenBuildingGetSharedStandardCertificateQuery
    {
        [Test, AutoData]
        public void Then_Query_Properties_Are_Set_Correctly(Guid id)
        {
            // Arrange & Act
            var query = new GetSharedStandardCertificateQuery(id);

            // Assert
            query.Id.Should().Be(id);
        }
    }
}
