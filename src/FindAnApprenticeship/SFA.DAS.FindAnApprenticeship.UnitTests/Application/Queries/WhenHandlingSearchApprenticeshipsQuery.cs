using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Assessors.UnitTests.Application.Queries
{
    public class WhenHandlingSearchApprenticeshipsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Called_And_Count_Returned(
            GetApprenticeshipCountResponse response,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
            SearchApprenticeshipsQueryHandler handler)
        {
            findApprenticeshipApiClient
                .Setup(x => x.Get<GetApprenticeshipCountResponse>(It.IsAny<GetApprenticeshipCountRequest>()))
                .ReturnsAsync(response);

            var actual = await handler.Handle(new SearchApprenticeshipsQuery(), CancellationToken.None);

            actual.TotalApprenticeshipCount.Should().Be(response.TotalVacancies);
        }

        [Test, MoqAutoData]
        public async Task And_when_There_Is_Filter_Data_Then_The_Api_Called_And_Count_Returned(
            SearchApprenticeshipsQuery query,
            LocationItem locationInfo,
            GetApprenticeshipCountResponse apiResponse,
            [Frozen] Mock<ILocationLookupService> locationLookupService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            SearchApprenticeshipsQueryHandler handler)
        {
            // Arrange
            locationLookupService
                .Setup(service => service.GetLocationInformation(
                    query.Location, default, default, false))
                .ReturnsAsync(locationInfo);

            // Pass locationInfo to the request
            var expectedRequest = new GetApprenticeshipCountRequest(
                locationInfo.GeoPoint?.FirstOrDefault(),
                locationInfo.GeoPoint?.LastOrDefault(),
                query.SelectedRouteIds,
                query.Distance
            );

            apiClient
                .Setup(client => client.Get<GetApprenticeshipCountResponse>(It.Is<GetApprenticeshipCountRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(apiResponse.TotalVacancies, result.TotalApprenticeshipCount);

        }
    }

}