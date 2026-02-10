using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetFrameworkCertificate;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetFrameworkCertificate
{
    public class WhenBuildingGetFrameworkCertificateQuery
    {
        [Test, AutoData]
        public void Then_Query_Properties_Are_Set_Correctly(Guid id)
        {
            // Arrange & Act
            var query = new GetFrameworkCertificateQuery(id);

            // Assert
            query.Id.Should().Be(id);
        }
    }
}
