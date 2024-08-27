using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using System;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests.RequestApprenticeTraining
{
    public class WhenBuildingTheAcknowledgeProviderResponsesRequest
    {
        [Test, AutoData]
        public void Then_The_PutUrl_Is_Correctly_Built(Guid employerRequestId, Guid acknowledgedBy)
        {
            // Arrange
            var request = new AcknowledgeProviderResponsesRequest(employerRequestId, new AcknowledgeProviderResponsesRequestData { AcknowledgedBy = acknowledgedBy });

            // Act
            var actualUrl = request.PutUrl;

            // Assert
            actualUrl.Should().Be($"api/employerrequest/{employerRequestId}/acknowledge-provider-responses");
        }

        [Test, AutoData]
        public void Then_The_Data_Is_Correctly_Assigned(Guid employerRequestId, AcknowledgeProviderResponsesRequestData data)
        {
            // Arrange & Act
            var request = new AcknowledgeProviderResponsesRequest(employerRequestId, data);

            // Assert
            request.Data.Should().BeEquivalentTo(data);
        }
    }
}
