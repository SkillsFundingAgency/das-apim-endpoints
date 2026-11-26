using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetStandardCertificate;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetStandardCertificate
{
    public class WhenBuildingGetStandardCertificateQuery
    {
        [Test, AutoData]
        public void Then_Query_Properties_Are_Set_Correctly(Guid id)
        {
            // Arrange & Act
            var query = new GetStandardCertificateQuery(id);

            // Assert
            query.Id.Should().Be(id);
        }
    }
}
