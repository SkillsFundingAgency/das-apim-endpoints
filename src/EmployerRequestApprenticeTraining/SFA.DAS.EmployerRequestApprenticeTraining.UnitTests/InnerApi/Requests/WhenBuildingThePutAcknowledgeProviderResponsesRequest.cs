using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingThePutAcknowledgeProviderResponsesRequest
    {
        [Test, AutoData]
        public void Then_The_PutUrl_Is_Correctly_Built(Guid employerRequestId, Guid acknowledgedBy)
        {
            // Arrange
            var request = new PutAcknowledgeProviderResponsesRequest(employerRequestId, new PutAcknowledgeProviderResponsesRequestData { AcknowledgedBy = acknowledgedBy });

            // Act
            var actualUrl = request.PutUrl;

            // Assert
            actualUrl.Should().Be($"api/employer-requests/{employerRequestId}/responses/acknowledge");
        }

        [Test, AutoData]
        public void Then_The_Data_Is_Correctly_Assigned(Guid employerRequestId, PutAcknowledgeProviderResponsesRequestData data)
        {
            // Arrange & Act
            var request = new PutAcknowledgeProviderResponsesRequest(employerRequestId, data);

            // Assert
            request.Data.Should().BeEquivalentTo(data);
        }
    }
}
