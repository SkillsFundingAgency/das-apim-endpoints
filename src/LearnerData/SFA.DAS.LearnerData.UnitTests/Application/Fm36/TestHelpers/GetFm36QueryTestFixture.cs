using AutoFixture;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.LearnerData.Application.Fm36;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using Episode = SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning.Episode;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

internal class GetFm36QueryTestFixture
{

    internal readonly Fixture Fixture = new();
    internal long Ukprn;
    internal byte CollectionPeriod;
    internal int CollectionYear;
    internal List<Learning> UnpagedLearningsResponse;
    internal GetFm36DataResponse EarningsResponse;
    internal GetAcademicYearsResponse CollectionCalendarResponse;
    internal Mock<ILearningApiClient<LearningApiConfiguration>> MockApprenticeshipsApiClient;
    internal Mock<IEarningsApiClient<EarningsApiConfiguration>> MockEarningsApiClient;
    internal Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> MockCollectionCalendarApiClient;
    internal Mock<IDistributedCache> MockDistributedCache;
    internal GetFm36Result Result;

    private GetFm36QueryHandler _handler;
    private GetFm36Query _query;

 
    internal GetFm36QueryTestFixture(TestScenario scenario)
    {
        // Arrange
        MockApprenticeshipsApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        MockEarningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
        MockCollectionCalendarApiClient = new Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>>();
        MockDistributedCache = new Mock<IDistributedCache>();

        Ukprn = Fixture.Create<long>();
        CollectionPeriod = 2;
        CollectionYear = 2425;

        var dataGenerator = new MockDataGenerator();
        dataGenerator.GenerateData(scenario);

        UnpagedLearningsResponse = dataGenerator.UnpagedLearningsResponse;
        EarningsResponse = dataGenerator.GetFm36DataResponse;

        CollectionCalendarResponse = BuildCollectionCalendarResponse(UnpagedLearningsResponse);
        SetupMocks(Ukprn, MockApprenticeshipsApiClient, UnpagedLearningsResponse, MockEarningsApiClient, EarningsResponse, MockCollectionCalendarApiClient, CollectionCalendarResponse, dataGenerator.SldLearnerData);

        _handler = new GetFm36QueryHandler(MockApprenticeshipsApiClient.Object, MockEarningsApiClient.Object, MockCollectionCalendarApiClient.Object, MockDistributedCache.Object, Mock.Of<ILogger<GetFm36QueryHandler>>());
        _query = new GetFm36Query(Ukprn, CollectionYear, CollectionPeriod, null, null);
    }

    internal GetAcademicYearsResponse BuildCollectionCalendarResponse(List<Learning> learningsResponse, bool apprenticeshipStartedInCurrentAcademicYear = true)
    {
        return new GetAcademicYearsResponse
        {
            AcademicYear = "2021",
            StartDate = new DateTime(2020, 8, 1),
            EndDate = new DateTime(2021, 7, 31)
        };
    }

    internal void SetupMocks(
        long ukprn,
        Mock<ILearningApiClient<LearningApiConfiguration>> mockApprenticeshipsApiClient,
        List<Learning> learningsResponse,
        Mock<IEarningsApiClient<EarningsApiConfiguration>> mockEarningsApiClient,
        GetFm36DataResponse earningsResponse,
        Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> mockCollectionCalendarApiClient,
        GetAcademicYearsResponse collectionCalendarResponse,
        List<UpdateLearnerRequest> sldLearnerData)
    {
        mockApprenticeshipsApiClient
            .Setup(x => x.Get<List<Learning>>(It.Is<GetLearningsRequest>(r => r.Ukprn == ukprn)))
            .ReturnsAsync(learningsResponse);

        mockApprenticeshipsApiClient
            .Setup(x => x.Get<GetPagedLearnersFromLearningInner>(It.Is<GetLearningsRequest>(r => r.Ukprn == ukprn)))
            .ReturnsAsync(new GetPagedLearnersFromLearningInner { Items = learningsResponse, Page = 1, PageSize = learningsResponse.Count, TotalItems = learningsResponse.Count });


        var response = new ApiResponse<GetFm36DataResponse>(earningsResponse, System.Net.HttpStatusCode.OK, string.Empty);
        mockEarningsApiClient
            .Setup(x => x.PostWithResponseCode<GetFm36DataResponse>(
                It.Is<PostGetFm36DataRequest>(r => r.Ukprn == ukprn), It.IsAny<bool>())).ReturnsAsync(response);

        MockCollectionCalendarApiClient
            .Setup(x => x.Get<GetAcademicYearsResponse>(It.Is<GetAcademicYearByYearRequest>(y => y.GetUrl == $"academicyears/{CollectionYear}")))
            .ReturnsAsync(collectionCalendarResponse);

        foreach (var learner in sldLearnerData)
        {
            var key = $"LearnerData_{ukprn}_{learner.Learner.Uln}";
            MockDistributedCache
                .Setup(x => x.GetAsync(It.Is<string>(k => k == key), It.IsAny<CancellationToken>()))
                .ReturnsAsync(System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(learner));
        }
    }

    public enum QueryType
    {
        Paged,
        Unpaged
    }

    internal async Task CallSubjectUnderTest(QueryType queryType = QueryType.Unpaged)
    {
        if(queryType == QueryType.Paged)
        {
            _query.Page = 1;
            _query.PageSize = 1;
        }

        // Act
        Result = await _handler.Handle(_query, CancellationToken.None);
    }

    internal IEnumerable<(Episode Episode, EpisodePrice Price)> GetExpectedPriceEpisodesSplitByAcademicYear(List<Episode> apprenticeshipEpisodes)
    {
        foreach (var episodePrice in apprenticeshipEpisodes
                     .SelectMany(episode => episode.Prices.Select(price => (Episode: episode, Price: price))))
        {
            if (episodePrice.Price.StartDate < CollectionCalendarResponse.StartDate)
            {
                var price = new EpisodePrice
                {
                    Key = episodePrice.Price.Key,
                    StartDate = CollectionCalendarResponse.StartDate,
                    EndDate = episodePrice.Price.EndDate,
                    EndPointAssessmentPrice = episodePrice.Price.EndPointAssessmentPrice,
                    FundingBandMaximum = episodePrice.Price.FundingBandMaximum,
                    TotalPrice = episodePrice.Price.TotalPrice,
                    TrainingPrice = episodePrice.Price.TrainingPrice
                };

                yield return new ValueTuple<Episode, EpisodePrice>(episodePrice.Episode, price);
            }
            else
            {
                yield return episodePrice;
            }
        }
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
