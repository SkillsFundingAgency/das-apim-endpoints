using System.Globalization;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearchVacancies;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.SavedSearches;

public class WhenHandlingGetSavedSearchVacanciesQuery
{
    [Test, MoqAutoData]
    public async Task When_Saved_Search_Results_Returns_Null_Then_Gets_Saved_Searches_Returns_Empty(
        Guid candidateId,
        double longitude,
        double latitude,
        GetSavedSearchVacanciesQuery mockQuery,
        GetSavedSearchesApiResponse mockGetSavedSearchesApiResponse,
        GetCandidateApiResponse getCandidateApiResponse,
        GetRoutesListResponse getRoutesListResponse,
        [Frozen] Mock<ICourseService> courseService,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockCandidateApiClient,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> mockFindApprenticeshipApiClient,
        GetSavedSearchVacanciesQueryHandler sut)
    {
        mockGetSavedSearchesApiResponse.SavedSearches = [];
        mockQuery.Latitude = latitude.ToString(CultureInfo.InvariantCulture);
        mockQuery.Longitude = longitude.ToString(CultureInfo.InvariantCulture);

        var expectedUrl = new GetSavedSearchesApiRequest(mockQuery.LastRunDateFilter.ToString("O"), mockQuery.PageNumber, mockQuery.PageSize);
        mockFindApprenticeshipApiClient.Setup(client => client.Get<GetSavedSearchesApiResponse>(It.Is<GetSavedSearchesApiRequest>(c => c.GetUrl == expectedUrl.GetUrl))).ReturnsAsync(mockGetSavedSearchesApiResponse);

        var candidateExpectedUrl = new GetCandidateApiRequest(candidateId.ToString());
        mockCandidateApiClient.Setup(client => client.Get<GetCandidateApiResponse>(It.Is<GetCandidateApiRequest>(c => c.GetUrl == candidateExpectedUrl.GetUrl))).ReturnsAsync(getCandidateApiResponse);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        actual.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task When_SearchParameters_Lat_Long_Route_Categories_Null_Then_Gets_Saved_Searches(
        double longitude,
        double latitude,
        GetSavedSearchVacanciesQuery mockQuery,
        GetCandidateApiResponse getCandidateApiResponse,
        GetRoutesListResponse getRoutesListResponse,
        GetVacanciesResponse getVacanciesResponse,
        GetCourseLevelsListResponse getCourseLevelsListResponse,
        [Frozen] Mock<ICourseService> courseService,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockCandidateApiClient,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> mockFindApprenticeshipApiClient,
        GetSavedSearchVacanciesQueryHandler sut)
    {
            
        mockQuery.Latitude = latitude.ToString(CultureInfo.InvariantCulture);
        mockQuery.Longitude = longitude.ToString(CultureInfo.InvariantCulture);
        
        getCandidateApiResponse.Status = UserStatus.Completed;

        courseService.Setup(x => x.GetRoutes())
            .ReturnsAsync(getRoutesListResponse);

        courseService.Setup(x => x.GetLevels())
            .ReturnsAsync(getCourseLevelsListResponse);

        var categories = getRoutesListResponse.Routes
            .Where(route =>
                mockQuery.SelectedRouteIds != null
                && mockQuery.SelectedRouteIds.Contains(route.Id))
            .Select(route => new GetSavedSearchVacanciesQueryResult.Category
            {
                Id = route.Id,
                Name = route.Name,
            }).ToList();

        var levels = getCourseLevelsListResponse.Levels
            .Where(level =>
                mockQuery.SelectedLevelIds != null &&
                mockQuery.SelectedLevelIds.Contains(level.Code))
            .Select(level => new GetSavedSearchVacanciesQueryResult.Level
            {
                Code = level.Code,
                Name = level.Name,
            }).ToList();

        
        var candidateExpectedUrl = new GetCandidateApiRequest(mockQuery.UserId.ToString());
        mockCandidateApiClient.Setup(client => client.Get<GetCandidateApiResponse>(It.Is<GetCandidateApiRequest>(c => c.GetUrl == candidateExpectedUrl.GetUrl))).ReturnsAsync(getCandidateApiResponse);

        var getVacanciesExpectedUrl = new GetVacanciesRequest(
            !string.IsNullOrEmpty(mockQuery.Latitude) ? Convert.ToDouble(mockQuery.Latitude) : null,
            !string.IsNullOrEmpty(mockQuery.Longitude) ? Convert.ToDouble(mockQuery.Longitude) : null,
            mockQuery.Distance,
            mockQuery.SearchTerm,
            1,  // Defaulting to top results.
            mockQuery.MaxApprenticeshipSearchResultsCount, // Default page size set to 5.
            categories.Select(cat => cat.Name!).ToList(),
            levels.Select(c=>c.Code).ToList(),
            mockQuery.ApprenticeshipSearchResultsSortOrder,
            mockQuery.DisabilityConfident,
            new List<VacancyDataSource> { VacancyDataSource.Nhs });

        mockFindApprenticeshipApiClient.Setup(client => client.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c => c.GetUrl == getVacanciesExpectedUrl.GetUrl))).ReturnsAsync(getVacanciesResponse);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        actual.Should().NotBeNull();
        actual.Vacancies.Should().NotBeEmpty();
        actual.Levels.Should().BeEquivalentTo(levels);
        actual.Categories.Should().BeEquivalentTo(categories);
    }
    
    [Test, MoqAutoData]
    public async Task When_Searching_Route_Ids_Are_Converted_To_The_Category_Name(
        double longitude,
        double latitude,
        GetSavedSearchVacanciesQuery query,
        GetCandidateApiResponse getCandidateApiResponse,
        GetRoutesListResponse getRoutesListResponse,
        GetCourseLevelsListResponse getCourseLevelsListResponse,
        [Frozen] Mock<ICourseService> courseService,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
        GetSavedSearchVacanciesQueryHandler sut)
    {
        // arrange
        query.Latitude = latitude.ToString(CultureInfo.InvariantCulture);
        query.Longitude = longitude.ToString(CultureInfo.InvariantCulture);
        courseService.Setup(x => x.GetRoutes()).ReturnsAsync(getRoutesListResponse);
        courseService.Setup(x => x.GetLevels()).ReturnsAsync(getCourseLevelsListResponse);
        candidateApiClient.Setup(client => client.Get<GetCandidateApiResponse>(It.IsAny<GetCandidateApiRequest>( ))).ReturnsAsync(getCandidateApiResponse);

        getCandidateApiResponse.Status = UserStatus.Completed;
        query.SelectedRouteIds = [getRoutesListResponse.Routes.First().Id];
        query.Latitude = null;
        query.Longitude = null;
        
        GetVacanciesRequest? request = null;
        findApprenticeshipApiClient
            .Setup(x => x.Get<GetVacanciesResponse>(It.IsAny<IGetApiRequest>()))
            .Callback<IGetApiRequest>(x => request = x as GetVacanciesRequest);
            
        // act
        await sut.Handle(query, It.IsAny<CancellationToken>());
        
        // assert
        request.Should().NotBeNull();
        request!.GetUrl.Should().Contain($"categories={getRoutesListResponse.Routes.First().Name}");
    }
}