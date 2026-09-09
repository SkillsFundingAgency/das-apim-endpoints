using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharedFrameworkCertificate;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetSharedFrameworkCertificate
{
    public class WhenBuildingGetSharedFrameworkCertificateQuery
    {
        [Test, AutoData]
        public void Then_Query_Properties_Are_Set_Correctly(Guid id)
        {
            // Arrange & Act
            var query = new GetSharedFrameworkCertificateQuery(id);

            // Assert
            query.Id.Should().Be(id);
        }
    }
}
