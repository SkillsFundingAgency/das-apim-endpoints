using System.Globalization;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearches;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.SavedSearches;

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
        [Frozen] Mock<ICourseService> courseService,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockCandidateApiClient,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> mockFindApprenticeshipApiClient,
        GetSavedSearchesQueryHandler sut)
    {
        foreach (var savedSearch in mockGetSavedSearchesApiResponse.SavedSearches)
        {
            savedSearch.UserReference = candidateId;
            savedSearch.SearchParameters.Latitude = longitude.ToString(CultureInfo.InvariantCulture);
            savedSearch.SearchParameters.Longitude = latitude.ToString(CultureInfo.InvariantCulture);
        }
        var expectedUrl = new GetSavedSearchesApiRequest(mockQuery.LastRunDateFilter.ToString("O"), mockQuery.PageNumber, mockQuery.PageSize);
        mockFindApprenticeshipApiClient.Setup(client => client.Get<GetSavedSearchesApiResponse>(It.Is<GetSavedSearchesApiRequest>(c => c.GetUrl == expectedUrl.GetUrl))).ReturnsAsync(mockGetSavedSearchesApiResponse);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        actual.Should().NotBeNull();
        actual.SavedSearchResults.Should().BeEquivalentTo(mockGetSavedSearchesApiResponse.SavedSearches.Select(c => (GetSavedSearchesQueryResult.SearchResult)c).ToList());
        actual.PageIndex.Should().Be(mockGetSavedSearchesApiResponse.PageIndex);
        actual.PageSize.Should().Be(mockGetSavedSearchesApiResponse.PageSize);
        actual.TotalPages.Should().Be(mockGetSavedSearchesApiResponse.TotalPages);
        actual.TotalCount.Should().Be(mockGetSavedSearchesApiResponse.TotalCount);
        actual.LastRunDateFilter.Should().Be(mockQuery.LastRunDateFilter);
    }
    
    [Test, MoqAutoData]
    public async Task Then_Returns_Empty_List_If_No_Saved_Searches(
        Guid candidateId,
        double longitude,
        double latitude,
        GetSavedSearchesQuery mockQuery,
        GetSavedSearchesApiResponse mockGetSavedSearchesApiResponse,
        [Frozen] Mock<ICourseService> courseService,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockCandidateApiClient,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> mockFindApprenticeshipApiClient,
        GetSavedSearchesQueryHandler sut)
    {
        mockGetSavedSearchesApiResponse.SavedSearches = [];
        foreach (var savedSearch in mockGetSavedSearchesApiResponse.SavedSearches)
        {
            savedSearch.UserReference = candidateId;
            savedSearch.SearchParameters.Latitude = longitude.ToString(CultureInfo.InvariantCulture);
            savedSearch.SearchParameters.Longitude = latitude.ToString(CultureInfo.InvariantCulture);
        }
        var expectedUrl = new GetSavedSearchesApiRequest(mockQuery.LastRunDateFilter.ToString("O"), mockQuery.PageNumber, mockQuery.PageSize);
        mockFindApprenticeshipApiClient
            .Setup(client =>
                client.Get<GetSavedSearchesApiResponse>(
                    It.Is<GetSavedSearchesApiRequest>(c => c.GetUrl == expectedUrl.GetUrl)))
            .ReturnsAsync(mockGetSavedSearchesApiResponse);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        actual.Should().NotBeNull();
        actual.SavedSearchResults.Should().BeEmpty();
        actual.PageIndex.Should().Be(mockGetSavedSearchesApiResponse.PageIndex);
        actual.PageSize.Should().Be(mockGetSavedSearchesApiResponse.PageSize);
        actual.TotalPages.Should().Be(mockGetSavedSearchesApiResponse.TotalPages);
        actual.TotalCount.Should().Be(mockGetSavedSearchesApiResponse.TotalCount);
        actual.LastRunDateFilter.Should().Be(mockQuery.LastRunDateFilter);
    }
}