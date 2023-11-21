using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Azure;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries
{
    public class WhenHandlingSearchApprenticeshipsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Services_Are_Called_And_Data_Returned_Based_On_Request(
            SearchApprenticeshipsQuery query,
            LocationItem locationInfo,
            GetApprenticeshipCountResponse apiResponse,
            GetVacanciesResponse vacanciesResponse,
            GetRoutesListResponse routesResponse,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<ILocationLookupService> locationLookupService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            SearchApprenticeshipsQueryHandler handler)
        {
            // Arrange
            locationLookupService
                .Setup(service => service.GetLocationInformation(
                    query.Location, default, default, false))
                .ReturnsAsync(locationInfo);
            courseService.Setup(x => x.GetRoutes()).ReturnsAsync(routesResponse);

            // Pass locationInfo to the request
            var expectedRequest = new GetApprenticeshipCountRequest(
                locationInfo.GeoPoint?.FirstOrDefault(),
                locationInfo.GeoPoint?.LastOrDefault(),
                query.SelectedRouteIds,
                query.Distance
            );

            var vacancyRequest = new GetVacanciesRequest(
                locationInfo.GeoPoint?.FirstOrDefault(),
                locationInfo.GeoPoint?.LastOrDefault(),
                query.SelectedRouteIds,
                query.Distance);

            apiClient
                .Setup(client => client.Get<GetApprenticeshipCountResponse>(It.Is<GetApprenticeshipCountRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            apiClient
                .Setup(client => client.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(r => r.GetUrl == vacancyRequest.GetUrl)))
                .ReturnsAsync(vacanciesResponse);


            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            result.TotalApprenticeshipCount.Should().Be(apiResponse.TotalVacancies);
            result.LocationItem.Should().BeEquivalentTo(locationInfo);
            result.Routes.Should().BeEquivalentTo(routesResponse.Routes);
            result.Vacancies.Should().BeEquivalentTo(vacanciesResponse.Vacancies);

        }
    }

}