using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetProvider;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetProviderQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Provider_From_Api_By_Ukprn(
            GetProviderQuery query,
            GetProviderSummaryResponse apiResponse,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpApiClient,
            GetProviderQueryHandler handler)
        {
            // Arrange
            var expectedUrl = $"api/providers/{query.Ukprn}";
            var response = new ApiResponse<GetProviderSummaryResponse>(apiResponse, HttpStatusCode.OK, string.Empty);

            mockRoatpApiClient
                .Setup(client => client.GetWithResponseCode<GetProviderSummaryResponse>(It.Is<GetRoatpProviderRequest>(r =>
                    r.GetUrl == expectedUrl)))
                .ReturnsAsync(response);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Provider.Should().BeEquivalentTo(apiResponse);
            mockRoatpApiClient.Verify(client => client.GetWithResponseCode<GetProviderSummaryResponse>(
                It.Is<GetRoatpProviderRequest>(r => r.GetUrl == expectedUrl)), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_Null_When_Provider_Not_Found(
           GetProviderQuery query,
           [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpApiClient,
           GetProviderQueryHandler handler)
        {
            // Arrange
            var expectedUrl = $"api/providers/{query.Ukprn}";
            var notFoundResponse = new ApiResponse<GetProviderSummaryResponse>(null, HttpStatusCode.NotFound, string.Empty);

            mockRoatpApiClient
                .Setup(client => client.GetWithResponseCode<GetProviderSummaryResponse>(It.Is<GetRoatpProviderRequest>(r =>
                    r.GetUrl == expectedUrl)))
                .ReturnsAsync(notFoundResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Provider.Should().BeNull();
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            GetProviderQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpApiClient,
            GetProviderQueryHandler handler)
        {
            // Arrange
            mockRoatpApiClient
                .Setup(client => client.GetWithResponseCode<GetProviderSummaryResponse>(It.IsAny<GetRoatpProviderRequest>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad Request"));

            // Act & Assert
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);
        }
    }
}
