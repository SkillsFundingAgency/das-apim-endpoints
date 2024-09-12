using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetAggregatedEmployerRequestsRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(long accountId)
        {
            // Arrange
            var request = new GetAggregatedEmployerRequestsRequest(accountId);

            // Act
            var actualUrl = request.GetUrl;

            // Assert
            actualUrl.Should().Be($"api/employerrequest/account/{accountId}/aggregated");
        }
    }
}
