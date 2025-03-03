using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Responses
{
    [TestFixture]
    public class WhenIGetAggregatedEmployerRequestsResponse
    {
        [Test]
        public void GetAggregatedEmployerRequestsResponse_PropertiesShouldBeSet()
        {
            // Arrange
            var response = new GetAggregatedEmployerRequestsResponse();

            // Act & Assert
            response.StandardReference.Should().BeNull();
            response.StandardLevel.Should().Be(0);
            response.StandardSector.Should().BeNull();
            response.StandardTitle.Should().BeNull();
            response.NumberOfApprentices.Should().Be(0);
            response.NumberOfEmployers.Should().Be(0);
        }
    }
}