using System.Collections.ObjectModel;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
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
            GetVacanciesResponse vacanciesResponse,
            GetApprenticeshipCountResponse apprenticeshipCountResponse,
            GetRoutesListResponse routesResponse,
            [Frozen] Mock<IMetrics> metricsService,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<ILocationLookupService> locationLookupService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            SearchApprenticeshipsQueryHandler handler)
        {
            // Arrange
            query.Sort = VacancySort.SalaryAsc;
            query.CandidateId = null;
            locationLookupService
                .Setup(service => service.GetLocationInformation(
                    query.Location, default, default, false))
                .ReturnsAsync(locationInfo);
            courseService.Setup(x => x.GetRoutes()).ReturnsAsync(routesResponse);

            var categories = routesResponse.Routes.Where(route => query.SelectedRouteIds != null && query.SelectedRouteIds.Contains(route.Id))
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
                query.SelectedLevelIds?.Select(c=>Convert.ToInt32(c)).ToList(),
                query.Sort,
                query.SkipWageType,
                query.DisabilityConfident,
                new List<VacancyDataSource>
                {
                    VacancyDataSource.Nhs
                });

            apiClient
                .Setup(client => client.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(r => r.GetUrl == vacancyRequest.GetUrl)))
                .ReturnsAsync(vacanciesResponse);

            var totalPages = (int)Math.Ceiling((double)vacanciesResponse.TotalFound / query.PageSize);

            var apprenticeCountRequest = new GetApprenticeshipCountRequest(WageType.CompetitiveSalary);
            apiClient.Setup(client =>
                    client.Get<GetApprenticeshipCountResponse>(
                        It.Is<GetApprenticeshipCountRequest>(r => r.GetUrl == apprenticeCountRequest.GetUrl)))
                .ReturnsAsync(apprenticeshipCountResponse);

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
                result.TotalWageTypeVacanciesCount.Should().Be(apprenticeshipCountResponse.TotalVacancies);
                metricsService.Verify(x => x.IncreaseVacancySearchResultViews(It.IsAny<string>(), 1), Times.Exactly(vacanciesResponse.ApprenticeshipVacancies.Count()));
                apiClient.Verify(client =>
                    client.Get<GetApprenticeshipCountResponse>(
                        It.IsAny<GetApprenticeshipCountRequest>()), Times.Once);
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
            List<int> routesIds,
            List<int> levelIds,
            Guid candidateId,
            LocationItem locationInfo,
            GetVacanciesResponse vacanciesResponse,
            GetRoutesListResponse routesResponse,
            GetApplicationsApiResponse getApplicationsApiResponse,
            GetSavedVacanciesApiResponse getSavedVacanciesApiResponse,
            GetApprenticeshipCountResponse apprenticeshipCountResponse,
            [Frozen] Mock<IMetrics> metricsService,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<ILocationLookupService> locationLookupService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            SearchApprenticeshipsQueryHandler handler)
        {
            // Arrange
            var query = new SearchApprenticeshipsQuery
            {
                CandidateId = candidateId,
                DisabilityConfident = true,
                Distance = 20,
                Location = "Hull",
                PageNumber = 2,
                PageSize = 20,
                SearchTerm = "Food",
                SelectedRouteIds = new ReadOnlyCollection<int>([1, 3]),
                SelectedLevelIds = new ReadOnlyCollection<int>([1, 2]),
                Sort = VacancySort.SalaryDesc
            };

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

            var categories = routesResponse.Routes.Where(route => query.SelectedRouteIds != null && query.SelectedRouteIds.Contains(route.Id))
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
                query.SelectedLevelIds?.Select(c => Convert.ToInt32(c)).ToList(),
                query.Sort,
                query.SkipWageType,
                query.DisabilityConfident,
                new List<VacancyDataSource>
                {
                    VacancyDataSource.Nhs
                });

            apiClient
                .Setup(client => client.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(r => r.GetUrl == vacancyRequest.GetUrl)))
                .ReturnsAsync(vacanciesResponse);

            var totalPages = (int)Math.Ceiling((double)vacanciesResponse.TotalFound / query.PageSize);

            var apprenticeCountRequest = new GetApprenticeshipCountRequest(WageType.CompetitiveSalary);
            apiClient.Setup(client =>
                    client.Get<GetApprenticeshipCountResponse>(
                        It.Is<GetApprenticeshipCountRequest>(r => r.GetUrl == apprenticeCountRequest.GetUrl)))
                .ReturnsAsync(apprenticeshipCountResponse);

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
                result.TotalWageTypeVacanciesCount.Should().Be(apprenticeshipCountResponse.TotalVacancies);
                metricsService.Verify(x => x.IncreaseVacancySearchResultViews(It.IsAny<string>(), 1), Times.Exactly(vacanciesResponse.ApprenticeshipVacancies.Count()));
                apiClient.Verify(client =>
                    client.Get<GetApprenticeshipCountResponse>(
                        It.IsAny<GetApprenticeshipCountRequest>()), Times.Once);
            }
        }

        [Test]
        [MoqInlineAutoData(VacancySort.AgeAsc)]
        [MoqInlineAutoData(VacancySort.AgeDesc)]
        [MoqInlineAutoData(VacancySort.ClosingAsc)]
        [MoqInlineAutoData(VacancySort.ClosingDesc)]
        [MoqInlineAutoData(VacancySort.DistanceAsc)]
        [MoqInlineAutoData(VacancySort.DistanceDesc)]
        [MoqInlineAutoData(VacancySort.ExpectedStartDateAsc)]
        [MoqInlineAutoData(VacancySort.ExpectedStartDateDesc)]
        public async Task Then_The_Sort_Other_than_Salary_ApprenticeCount_Never_Called_And_Data_Returned_Based_On_Request(
           VacancySort sort,
           Guid candidateId,
           LocationItem locationInfo,
           GetVacanciesResponse vacanciesResponse,
           GetRoutesListResponse routesResponse,
           GetApplicationsApiResponse getApplicationsApiResponse,
           GetSavedVacanciesApiResponse getSavedVacanciesApiResponse,
           GetCandidateSavedSearchesApiResponse getSavedSearchesApiResponse,
           [Frozen] Mock<IMetrics> metricsService,
           [Frozen] Mock<ICourseService> courseService,
           [Frozen] Mock<ILocationLookupService> locationLookupService,
           [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
           [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
           SearchApprenticeshipsQueryHandler handler)
        {
            // Arrange
            var query = new SearchApprenticeshipsQuery
            {
                CandidateId = candidateId,
                DisabilityConfident = true,
                Distance = 20,
                Location = "Hull",
                PageNumber = 2,
                PageSize = 20,
                SearchTerm = "Food",
                SelectedRouteIds = new ReadOnlyCollection<int>([1, 3]),
                SelectedLevelIds = new ReadOnlyCollection<int>([1, 2]),
                Sort = VacancySort.DistanceAsc
            };
                        
            query.Sort = sort;
            query.CandidateId = candidateId;
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

            var categories = routesResponse.Routes.Where(route => query.SelectedRouteIds != null && query.SelectedRouteIds.Contains(route.Id))
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
                query.SelectedLevelIds?.Select(c => Convert.ToInt32(c)).ToList(),
                query.Sort,
                query.SkipWageType,
                query.DisabilityConfident,
                new List<VacancyDataSource>
                {
                    VacancyDataSource.Nhs
                });

            apiClient
                .Setup(client => client.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(r => r.GetUrl == vacancyRequest.GetUrl)))
                .ReturnsAsync(vacanciesResponse);

            apiClient
                .Setup(client => client.Get<GetCandidateSavedSearchesApiResponse>(
                    It.Is<GetCandidateSavedSearchesApiRequest>(r => r.GetUrl == $"api/Users/{candidateId}/SavedSearches")))
                .ReturnsAsync(getSavedSearchesApiResponse);

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
                result.SavedSearchesCount.Should().Be(3);
                result.SearchAlreadySaved.Should().BeFalse();
                result.TotalWageTypeVacanciesCount.Should().Be(0);
                metricsService.Verify(x => x.IncreaseVacancySearchResultViews(It.IsAny<string>(), 1), Times.Exactly(vacanciesResponse.ApprenticeshipVacancies.Count()));
                apiClient.Verify(client =>
                    client.Get<GetApprenticeshipCountResponse>(
                        It.IsAny<GetApprenticeshipCountRequest>()), Times.Never);
            }
        }
    }
}