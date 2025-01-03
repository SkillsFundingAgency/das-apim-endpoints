﻿using System.Globalization;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearches;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using GetVacanciesRequest = SFA.DAS.SharedOuterApi.InnerApi.Requests.GetVacanciesRequest;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.SavedSearches
{
    [TestFixture]
    public class WhenHandlingGetSavedSearchesQueryHandler
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Saved_Searches(
            Guid candidateId,
            double longitude,
            double latitude,
            GetSavedSearchesQuery mockQuery,
            GetSavedSearchesApiResponse mockGetSavedSearchesApiResponse,
            GetCandidateApiResponse getCandidateApiResponse,
            GetRoutesListResponse getRoutesListResponse,
            GetCourseLevelsListResponse getCourseLevelsListResponse,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockCandidateApiClient,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> mockFindApprenticeshipApiClient,
            GetSavedSearchesQueryHandler sut)
        {
            getCandidateApiResponse.Status = UserStatus.Completed;

            foreach (var savedSearch in mockGetSavedSearchesApiResponse.SavedSearches)
            {
                savedSearch.UserReference = candidateId;
                savedSearch.SearchParameters.Latitude = longitude.ToString(CultureInfo.InvariantCulture);
                savedSearch.SearchParameters.Longitude = latitude.ToString(CultureInfo.InvariantCulture);
            }

            var expectedUrl = new GetSavedSearchesApiRequest(mockQuery.LastRunDateFilter.ToString("O"), mockQuery.PageNumber, mockQuery.PageSize);
            mockFindApprenticeshipApiClient.Setup(client => client.Get<GetSavedSearchesApiResponse>(It.Is<GetSavedSearchesApiRequest>(c => c.GetUrl == expectedUrl.GetUrl))).ReturnsAsync(mockGetSavedSearchesApiResponse);

            var candidateExpectedUrl = new GetCandidateApiRequest(candidateId.ToString());
            mockCandidateApiClient.Setup(client => client.Get<GetCandidateApiResponse>(It.Is<GetCandidateApiRequest>(c => c.GetUrl == candidateExpectedUrl.GetUrl))).ReturnsAsync(getCandidateApiResponse);

            courseService.Setup(x => x.GetRoutes())
                .ReturnsAsync(getRoutesListResponse);

            courseService.Setup(x => x.GetLevels())
                .ReturnsAsync(getCourseLevelsListResponse);

            var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

            actual.Should().NotBeNull();
            //actual.SavedSearchResults.Count.Should().Be(mockGetSavedSearchesApiResponse.SavedSearches.Count);
            actual.SavedSearchResults.FirstOrDefault()?.User.Should().NotBeNull();
            actual.SavedSearchResults.FirstOrDefault()?.User?.Should().BeEquivalentTo(getCandidateApiResponse, options => options.Excluding(ex => ex.Status));
            actual.PageIndex.Should().Be(mockGetSavedSearchesApiResponse.PageIndex);
            actual.PageSize.Should().Be(mockGetSavedSearchesApiResponse.PageSize);
            actual.TotalPages.Should().Be(mockGetSavedSearchesApiResponse.TotalPages);
            actual.TotalCount.Should().Be(mockGetSavedSearchesApiResponse.TotalCount);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Vacancies_Not_Matches_Saved_Searches_Returns_Empty(
            Guid candidateId,
            double longitude,
            double latitude,
            GetSavedSearchesQuery mockQuery,
            GetSavedSearchesApiResponse mockGetSavedSearchesApiResponse,
            GetCandidateApiResponse getCandidateApiResponse,
            GetRoutesListResponse getRoutesListResponse,
            GetCourseLevelsListResponse getCourseLevelsListResponse,
            GetVacanciesResponse getVacanciesResponse,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockCandidateApiClient,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> mockFindApprenticeshipApiClient,
            GetSavedSearchesQueryHandler sut)
        {
            getCandidateApiResponse.Status = UserStatus.Completed;

            foreach (var savedSearch in mockGetSavedSearchesApiResponse.SavedSearches)
            {
                savedSearch.UserReference = candidateId;
                savedSearch.SearchParameters.Latitude = longitude.ToString(CultureInfo.InvariantCulture);
                savedSearch.SearchParameters.Longitude = latitude.ToString(CultureInfo.InvariantCulture);
            }

            var expectedUrl = new GetSavedSearchesApiRequest(mockQuery.LastRunDateFilter.ToString("O"), mockQuery.PageNumber, mockQuery.PageSize);
            mockFindApprenticeshipApiClient.Setup(client => client.Get<GetSavedSearchesApiResponse>(It.Is<GetSavedSearchesApiRequest>(c => c.GetUrl == expectedUrl.GetUrl))).ReturnsAsync(mockGetSavedSearchesApiResponse);

            var candidateExpectedUrl = new GetCandidateApiRequest(candidateId.ToString());
            mockCandidateApiClient.Setup(client => client.Get<GetCandidateApiResponse>(It.Is<GetCandidateApiRequest>(c => c.GetUrl == candidateExpectedUrl.GetUrl))).ReturnsAsync(getCandidateApiResponse);

            mockFindApprenticeshipApiClient.Setup(client => client.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c => c.GetUrl == It.IsAny<string>()))).ReturnsAsync((GetVacanciesResponse)null!);

            courseService.Setup(x => x.GetRoutes())
                .ReturnsAsync(getRoutesListResponse);

            courseService.Setup(x => x.GetLevels())
                .ReturnsAsync(getCourseLevelsListResponse);

            var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

            actual.Should().NotBeNull();
            actual.SavedSearchResults.Count.Should().Be(3);
            actual.SavedSearchResults.TrueForAll(c => c.Vacancies.Count == 0).Should().BeTrue();
            actual.SavedSearchResults.FirstOrDefault()?.User.Should().NotBeNull();
            actual.SavedSearchResults.FirstOrDefault()?.User?.Should().BeEquivalentTo(getCandidateApiResponse, options => options.Excluding(ex => ex.Status));
            actual.PageIndex.Should().Be(mockGetSavedSearchesApiResponse.PageIndex);
            actual.PageSize.Should().Be(mockGetSavedSearchesApiResponse.PageSize);
            actual.TotalPages.Should().Be(mockGetSavedSearchesApiResponse.TotalPages);
            actual.TotalCount.Should().Be(mockGetSavedSearchesApiResponse.TotalCount);
        }

        [Test, MoqAutoData]
        public async Task When_Candidate_Account_Disabled_Then_Gets_Saved_Searches_Returns_Empty(
            Guid candidateId,
            GetSavedSearchesQuery mockQuery,
            GetSavedSearchesApiResponse mockGetSavedSearchesApiResponse,
            GetCandidateApiResponse getCandidateApiResponse,
            GetRoutesListResponse getRoutesListResponse,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockCandidateApiClient,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> mockFindApprenticeshipApiClient,
            GetSavedSearchesQueryHandler sut)
        {
            foreach (var savedSearch in mockGetSavedSearchesApiResponse.SavedSearches)
            {
                savedSearch.UserReference = candidateId;
            }

            getCandidateApiResponse.Status = UserStatus.Deleted;

            var expectedUrl = new GetSavedSearchesApiRequest(mockQuery.LastRunDateFilter.ToString("O"), mockQuery.PageNumber, mockQuery.PageSize);
            mockFindApprenticeshipApiClient.Setup(client => client.Get<GetSavedSearchesApiResponse>(It.Is<GetSavedSearchesApiRequest>(c => c.GetUrl == expectedUrl.GetUrl))).ReturnsAsync(mockGetSavedSearchesApiResponse);

            var candidateExpectedUrl = new GetCandidateApiRequest(candidateId.ToString());
            mockCandidateApiClient.Setup(client => client.Get<GetCandidateApiResponse>(It.Is<GetCandidateApiRequest>(c => c.GetUrl == candidateExpectedUrl.GetUrl))).ReturnsAsync(getCandidateApiResponse);

            var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

            actual.Should().NotBeNull();
            actual.SavedSearchResults.Count.Should().Be(0);
            actual.PageIndex.Should().Be(mockGetSavedSearchesApiResponse.PageIndex);
            actual.PageSize.Should().Be(mockGetSavedSearchesApiResponse.PageSize);
            actual.TotalPages.Should().Be(mockGetSavedSearchesApiResponse.TotalPages);
            actual.TotalCount.Should().Be(mockGetSavedSearchesApiResponse.TotalCount);
        }

        [Test, MoqAutoData]
        public async Task When_Saved_Search_Results_Returns_Null_Then_Gets_Saved_Searches_Returns_Empty(
            Guid candidateId,
            GetSavedSearchesQuery mockQuery,
            GetSavedSearchesApiResponse mockGetSavedSearchesApiResponse,
            GetCandidateApiResponse getCandidateApiResponse,
            GetRoutesListResponse getRoutesListResponse,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockCandidateApiClient,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> mockFindApprenticeshipApiClient,
            GetSavedSearchesQueryHandler sut)
        {
            mockGetSavedSearchesApiResponse.SavedSearches = [];

            var expectedUrl = new GetSavedSearchesApiRequest(mockQuery.LastRunDateFilter.ToString("O"), mockQuery.PageNumber, mockQuery.PageSize);
            mockFindApprenticeshipApiClient.Setup(client => client.Get<GetSavedSearchesApiResponse>(It.Is<GetSavedSearchesApiRequest>(c => c.GetUrl == expectedUrl.GetUrl))).ReturnsAsync(mockGetSavedSearchesApiResponse);

            var candidateExpectedUrl = new GetCandidateApiRequest(candidateId.ToString());
            mockCandidateApiClient.Setup(client => client.Get<GetCandidateApiResponse>(It.Is<GetCandidateApiRequest>(c => c.GetUrl == candidateExpectedUrl.GetUrl))).ReturnsAsync(getCandidateApiResponse);

            var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

            actual.Should().NotBeNull();
            actual.SavedSearchResults.Count.Should().Be(0);
            actual.PageIndex.Should().Be(mockGetSavedSearchesApiResponse.PageIndex);
            actual.PageSize.Should().Be(mockGetSavedSearchesApiResponse.PageSize);
            actual.TotalPages.Should().Be(mockGetSavedSearchesApiResponse.TotalPages);
            actual.TotalCount.Should().Be(mockGetSavedSearchesApiResponse.TotalCount);
        }

        [Test, MoqAutoData]
        public async Task When_SearchParameters_Lat_Long_Route_Categories_Null_Then_Gets_Saved_Searches(
            Guid candidateId,
            GetSavedSearchesQuery mockQuery,
            GetSavedSearchesApiResponse mockGetSavedSearchesApiResponse,
            GetCandidateApiResponse getCandidateApiResponse,
            GetRoutesListResponse getRoutesListResponse,
            GetVacanciesResponse getVacanciesResponse,
            GetCourseLevelsListResponse getCourseLevelsListResponse,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockCandidateApiClient,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> mockFindApprenticeshipApiClient,
            GetSavedSearchesQueryHandler sut)
        {
            if (mockGetSavedSearchesApiResponse.SavedSearches.Count > 1)
                mockGetSavedSearchesApiResponse.SavedSearches.RemoveRange(1, mockGetSavedSearchesApiResponse.SavedSearches.Count - 1);

            getCandidateApiResponse.Status = UserStatus.Completed;

            var savedSearch = mockGetSavedSearchesApiResponse.SavedSearches.FirstOrDefault();
            savedSearch!.UserReference = candidateId;
            savedSearch.SearchParameters.Latitude = null;
            savedSearch.SearchParameters.Longitude = null;
            savedSearch.SearchParameters.SelectedRouteIds = [];
            savedSearch.SearchParameters.SelectedLevelIds = [];
            savedSearch.SearchParameters.Distance = null;

            courseService.Setup(x => x.GetRoutes())
                .ReturnsAsync(getRoutesListResponse);

            courseService.Setup(x => x.GetLevels())
                .ReturnsAsync(getCourseLevelsListResponse);

            var categories = getRoutesListResponse.Routes
                .Where(route =>
                    savedSearch.SearchParameters.SelectedRouteIds != null
                    && savedSearch.SearchParameters.SelectedRouteIds.Contains(route.Id))
                .Select(route => new GetSavedSearchesQueryResult.SearchResult.Category
                {
                    Id = route.Id,
                    Name = route.Name,
                }).ToList();

            var levels = getCourseLevelsListResponse.Levels
                .Where(level =>
                    savedSearch.SearchParameters.SelectedLevelIds != null &&
                    savedSearch.SearchParameters.SelectedLevelIds.Contains(level.Code))
                .Select(level => new GetSavedSearchesQueryResult.SearchResult.Level
                {
                    Code = level.Code,
                    Name = level.Name,
                }).ToList();

            var expectedUrl = new GetSavedSearchesApiRequest(mockQuery.LastRunDateFilter.ToString("O"), mockQuery.PageNumber, mockQuery.PageSize);
            mockFindApprenticeshipApiClient.Setup(client => client.Get<GetSavedSearchesApiResponse>(It.Is<GetSavedSearchesApiRequest>(c => c.GetUrl == expectedUrl.GetUrl))).ReturnsAsync(mockGetSavedSearchesApiResponse);

            var candidateExpectedUrl = new GetCandidateApiRequest(candidateId.ToString());
            mockCandidateApiClient.Setup(client => client.Get<GetCandidateApiResponse>(It.Is<GetCandidateApiRequest>(c => c.GetUrl == candidateExpectedUrl.GetUrl))).ReturnsAsync(getCandidateApiResponse);

            var getVacanciesExpectedUrl = new FindApprenticeshipJobs.InnerApi.Requests.GetVacanciesRequest(
                !string.IsNullOrEmpty(savedSearch.SearchParameters.Latitude) ? Convert.ToDouble(savedSearch.SearchParameters.Latitude) : null,
                !string.IsNullOrEmpty(savedSearch.SearchParameters.Longitude) ? Convert.ToDouble(savedSearch.SearchParameters.Longitude) : null,
                savedSearch.SearchParameters.Distance,
                savedSearch.SearchParameters.SearchTerm,
                1,  // Defaulting to top results.
                mockQuery.MaxApprenticeshipSearchResultsCount, // Default page size set to 5.
                categories.Select(cat => cat.Id.ToString()).ToList(),
                savedSearch.SearchParameters.SelectedLevelIds,
                mockQuery.ApprenticeshipSearchResultsSortOrder,
                savedSearch.SearchParameters.DisabilityConfident,
                new List<VacancyDataSource> { VacancyDataSource.Nhs });

            mockFindApprenticeshipApiClient.Setup(client => client.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c => c.GetUrl == getVacanciesExpectedUrl.GetUrl))).ReturnsAsync(getVacanciesResponse);

            var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

            actual.Should().NotBeNull();
            actual.PageIndex.Should().Be(mockGetSavedSearchesApiResponse.PageIndex);
            actual.PageSize.Should().Be(mockGetSavedSearchesApiResponse.PageSize);
            actual.TotalPages.Should().Be(mockGetSavedSearchesApiResponse.TotalPages);
            actual.TotalCount.Should().Be(mockGetSavedSearchesApiResponse.TotalCount);
        }
    }
}
