using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using static SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining.SubmitEmployerRequestRequest;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests.RequestApprenticeTraining
{
    public class WhenBuildingTheSubmitEmployerRequestRequest
    {
        [Test, AutoData]
        public void Then_The_PostUrl_Is_Correctly_Built(
            long accountId, 
            SubmitEmployerRequestData data)
        {
            // Arrange
            var request = new SubmitEmployerRequestRequest(accountId, data);

            // Act & Assert
            request.PostUrl.Should().Be($"api/employerrequest/account/{accountId}/submit-request");
        }

        [Test, AutoData]
        public void Then_The_Data_Is_Correctly_Assigned(
            long accountId,
            SubmitEmployerRequestData data)
        {
            // Arrange & Act
            var request = new SubmitEmployerRequestRequest(accountId, data);

            // Assert
            request.Data.Should().BeEquivalentTo(data);
        }
    }
}
