using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetCertificatesMatch;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetCertificatesMatch
{
    public class WhenCreatingGetCertificatesMatchQuery
    {
        [Test]
        public void Then_Query_Properties_Are_Set_Correctly()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var query = new GetCertificatesMatchQuery { UserId = userId };

            // Assert
            query.UserId.Should().Be(userId);
        }
    }
}
