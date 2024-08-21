using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using System;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests.RequestApprenticeTraining
{
    public class WhenBuildingTheGetEmployerRequestRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built_With_EmployerRequestId(Guid employerRequestId)
        {
            // Arrange
            var request = new GetEmployerRequestRequest(employerRequestId);

            // Act
            var actualUrl = request.GetUrl;

            // Assert
            actualUrl.Should().Be($"api/employerrequest/{employerRequestId}");
        }

        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built_With_AccountId_And_StandardReference(long accountId, string standardReference)
        {
            // Arrange
            var request = new GetEmployerRequestRequest(accountId, standardReference);

            // Act
            var actualUrl = request.GetUrl;

            // Assert
            actualUrl.Should().Be($"api/employerrequest/account/{accountId}/standard/{standardReference}");
        }
    }
}
