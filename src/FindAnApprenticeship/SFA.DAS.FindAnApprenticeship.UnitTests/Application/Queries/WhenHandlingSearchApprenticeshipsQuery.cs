using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries
{
    public class WhenHandlingSearchApprenticeshipsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Services_Are_Called_And_Data_Returned_Based_On_Request(
            SearchApprenticeshipsQuery query,
            LocationItem locationInfo,
            GetVacanciesResponse vacanciesResponse,
            GetRoutesListResponse routesResponse,
            [Frozen] Mock<IMetrics> metricsService,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<ILocationLookupService> locationLookupService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            SearchApprenticeshipsQueryHandler handler)
        {
            // Arrange
            query.CandidateId = string.Empty;
            locationLookupService
                .Setup(service => service.GetLocationInformation(
                    query.Location, default, default, false))
                .ReturnsAsync(locationInfo);
            courseService.Setup(x => x.GetRoutes()).ReturnsAsync(routesResponse);

            var categories = routesResponse.Routes.Where(route => query.SelectedRouteIds != null && query.SelectedRouteIds.Contains(route.Id.ToString()))
                .Select(route => route.Name).ToList();

            // Pass locationInfo to the request
            var vacancyRequest = new GetVacanciesRequest(
                locationInfo.GeoPoint?.FirstOrDefault(),
                locationInfo.GeoPoint?.LastOrDefault(),
                query.Distance,
                query.SearchTerm,
                query.PageNumber,
                query.PageSize,
                categories,
                query.SelectedLevelIds,
                query.Sort,
                query.DisabilityConfident);

            apiClient
                .Setup(client => client.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(r => r.GetUrl == vacancyRequest.GetUrl)))
                .ReturnsAsync(vacanciesResponse);

            var totalPages = (int)Math.Ceiling((double)vacanciesResponse.TotalFound / query.PageSize);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                Assert.That(result, Is.Not.Null);
                result.TotalApprenticeshipCount.Should().Be(vacanciesResponse.Total);
                result.TotalFound.Should().Be(vacanciesResponse.TotalFound);
                result.LocationItem.Should().BeEquivalentTo(locationInfo);
                result.Routes.Should().BeEquivalentTo(routesResponse.Routes);
                result.Vacancies.Should().BeEquivalentTo(vacanciesResponse.ApprenticeshipVacancies);
                result.PageNumber.Should().Be(query.PageNumber);
                result.PageSize.Should().Be(query.PageSize);
                result.TotalPages.Should().Be(totalPages);
                result.DisabilityConfident.Should().Be(query.DisabilityConfident);
                metricsService.Verify(x => x.IncreaseVacancySearchResultViews(It.IsAny<string>(), 1), Times.Exactly(vacanciesResponse.ApprenticeshipVacancies.Count()));
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

            Assert.That(result, Is.Not.Null);
            result.VacancyReference.Should().Be(query.SearchTerm);

        }

        [Test, MoqAutoData]
        public async Task Then_The_Services_Are_Called_And_Data_Returned_Based_On_Request_When_Candidate_Id_Given(
            Guid candidateId,
            SearchApprenticeshipsQuery query,
            LocationItem locationInfo,
            GetVacanciesResponse vacanciesResponse,
            GetRoutesListResponse routesResponse,
            GetApplicationsApiResponse getApplicationsApiResponse,
            GetSavedVacanciesApiResponse getSavedVacanciesApiResponse,
            [Frozen] Mock<IMetrics> metricsService,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<ILocationLookupService> locationLookupService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            SearchApprenticeshipsQueryHandler handler)
        {
            // Arrange
            query.CandidateId = candidateId.ToString();
            locationLookupService
                .Setup(service => service.GetLocationInformation(
                    query.Location, default, default, false))
                .ReturnsAsync(locationInfo);
            courseService.Setup(x => x.GetRoutes()).ReturnsAsync(routesResponse);

            var expectedUrl = new GetApplicationsApiRequest(candidateId);
            candidateApiClient.Setup(service =>
                    service.Get<GetApplicationsApiResponse>(
                        It.Is<GetApplicationsApiRequest>(r => r.GetUrl == expectedUrl.GetUrl)))
                .ReturnsAsync(getApplicationsApiResponse);

            var expectedSavedApplicationsApiRequestUrl = new GetSavedVacanciesApiRequest(candidateId);
            candidateApiClient.Setup(service =>
                    service.Get<GetSavedVacanciesApiResponse>(
                        It.Is<GetSavedVacanciesApiRequest>(r => r.GetUrl == expectedSavedApplicationsApiRequestUrl.GetUrl)))
                .ReturnsAsync(getSavedVacanciesApiResponse);

            var categories = routesResponse.Routes.Where(route => query.SelectedRouteIds != null && query.SelectedRouteIds.Contains(route.Id.ToString()))
                .Select(route => route.Name).ToList();

            // Pass locationInfo to the request
            var vacancyRequest = new GetVacanciesRequest(
                locationInfo.GeoPoint?.FirstOrDefault(),
                locationInfo.GeoPoint?.LastOrDefault(),
                query.Distance,
                query.SearchTerm,
                query.PageNumber,
                query.PageSize,
                categories,
                query.SelectedLevelIds,
                query.Sort,
                query.DisabilityConfident);

            apiClient
                .Setup(client => client.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(r => r.GetUrl == vacancyRequest.GetUrl)))
                .ReturnsAsync(vacanciesResponse);

            var totalPages = (int)Math.Ceiling((double)vacanciesResponse.TotalFound / query.PageSize);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                Assert.That(result, Is.Not.Null);
                result.TotalFound.Should().Be(vacanciesResponse.TotalFound);
                result.LocationItem.Should().BeEquivalentTo(locationInfo);
                result.Routes.Should().BeEquivalentTo(routesResponse.Routes);
                result.Vacancies.Should().BeEquivalentTo(vacanciesResponse.ApprenticeshipVacancies);
                result.PageNumber.Should().Be(query.PageNumber);
                result.PageSize.Should().Be(query.PageSize);
                result.TotalPages.Should().Be(totalPages);
                result.DisabilityConfident.Should().Be(query.DisabilityConfident);
                metricsService.Verify(x => x.IncreaseVacancySearchResultViews(It.IsAny<string>(), 1), Times.Exactly(vacanciesResponse.ApprenticeshipVacancies.Count()));
            }
        }
    }
}