using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests.RequestApprenticeTraining
{
    public class WhenBuildingTheGetEmployerRequestsRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(long accountId)
        {
            // Arrange
            var request = new GetEmployerRequestsRequest(accountId);

            // Act
            var actualUrl = request.GetUrl;

            // Assert
            actualUrl.Should().Be($"api/employerrequest/account/{accountId}");
        }
    }
}
