using AutoFixture.NUnit3;
using Azure.Core;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetStandard;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GetStandardRequest = SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests.GetStandardRequest;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetStandard
    {
        [Test, MoqAutoData]
        public async Task Then_Get_Standard_From_The_Api(
           StandardResponse standardResponse,
           GetStandardQuery query,
           [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
           GetStandardQueryHandler handler)
        {
            // Arrange
            var response = new ApiResponse<StandardResponse>(standardResponse, HttpStatusCode.OK, string.Empty);

            mockRequestApprenticeTrainingClient
                .Setup(client => client.GetWithResponseCode<StandardResponse>(It.IsAny<GetStandardRequest>()))
            .ReturnsAsync(response);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Standard.Should().BeEquivalentTo(standardResponse);
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            GetStandardQuery query,
            [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
            GetStandardQueryHandler handler)
        {
            // Arrange
            mockRequestApprenticeTrainingClient
               .Setup(client => client.GetWithResponseCode<StandardResponse>(It.IsAny<GetStandardRequest>()))
               .ThrowsAsync(new ApiResponseException(HttpStatusCode.NotFound, "Not Found"));

            // Act & Assert
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.NotFound);
        }
    }
}
