using AutoFixture.NUnit3;
using Azure.Core;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NServiceBus.Timeout.Core;
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

            var categories = routesResponse.Routes.Where(route => query.SelectedRouteIds != null && query.SelectedRouteIds.Contains(route.Id.ToString()))
                .Select(route => route.Name).ToList();

            // Pass locationInfo to the request
            var expectedRequest = new GetApprenticeshipCountRequest(
                locationInfo.GeoPoint?.FirstOrDefault(),
                locationInfo.GeoPoint?.LastOrDefault(),
                query.Distance,
                categories,
                query.SearchTerm
            );

            var vacancyRequest = new GetVacanciesRequest(
                locationInfo.GeoPoint?.FirstOrDefault(),
                locationInfo.GeoPoint?.LastOrDefault(),
                query.Distance,
                query.SearchTerm,
                query.PageNumber,
                query.PageSize,
                categories,
                query.Sort);

            apiClient
                .Setup(client => client.Get<GetApprenticeshipCountResponse>(It.Is<GetApprenticeshipCountRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            apiClient
                .Setup(client => client.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(r => r.GetUrl == vacancyRequest.GetUrl)))
                .ReturnsAsync(vacanciesResponse);

            var totalPages = (int)Math.Ceiling((double)apiResponse.TotalVacancies / query.PageSize);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                Assert.NotNull(result);
                result.TotalApprenticeshipCount.Should().Be(apiResponse.TotalVacancies);
                result.LocationItem.Should().BeEquivalentTo(locationInfo);
                result.Routes.Should().BeEquivalentTo(routesResponse.Routes);
                result.Vacancies.Should().BeEquivalentTo(vacanciesResponse.ApprenticeshipVacancies);
                result.PageNumber.Should().Be(query.PageNumber);
                result.PageSize.Should().Be(query.PageSize);
                result.TotalPages.Should().Be(totalPages);
            }
        }

        [Test, MoqAutoData]
        public async Task Then_The_Search_Term_Is_A_Vacancy_Reference_And_Vacancy_Reference_Is_Returned(
            SearchApprenticeshipsQuery query, 
            GetApprenticeshipVacancyItemResponse apiResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            SearchApprenticeshipsQueryHandler handler
            )
        {
            query.SearchTerm = "VAC1098765465";

            var expectedRequest = new GetVacancyRequest(query.SearchTerm);

            apiClient
                .Setup(client => client.Get<GetApprenticeshipVacancyItemResponse>(It.Is<GetVacancyRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            result.VacancyReference.Should().Be(query.SearchTerm);

        }
    }
}