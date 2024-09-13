using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using static SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests.PostSubmitEmployerRequestRequest;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingThePostSubmitEmployerRequestRequest
    {
        [Test, AutoData]
        public void Then_The_PostUrl_Is_Correctly_Built(
            long accountId,
            PostSubmitEmployerRequestData data)
        {
            // Arrange
            var request = new PostSubmitEmployerRequestRequest(accountId, data);

            // Act & Assert
            request.PostUrl.Should().Be($"api/accounts/{accountId}/employer-requests");
        }

        [Test, AutoData]
        public void Then_The_Data_Is_Correctly_Assigned(
            long accountId,
            PostSubmitEmployerRequestData data)
        {
            // Arrange & Act
            var request = new PostSubmitEmployerRequestRequest(accountId, data);

            // Assert
            request.Data.Should().BeEquivalentTo(data);
        }
    }
}
