using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Earnings.Application.Earnings;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Earnings.UnitTests.MockDataGenerator;
using Episode = SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning.Episode;

namespace SFA.DAS.Earnings.UnitTests.Application.Earnings;

public class GetAllEarningsQueryTestFixture
{
        
    public readonly Fixture Fixture = new();
    public long Ukprn;
    public byte CollectionPeriod;
    public int CollectionYear;
    public GetLearningsResponse LearningsResponse;
    public GetFm36DataResponse EarningsResponse;
    public GetAcademicYearsResponse CollectionCalendarResponse;
    public Mock<ILearningApiClient<LearningApiConfiguration>> MockApprenticeshipsApiClient;
    public Mock<IEarningsApiClient<EarningsApiConfiguration>> MockEarningsApiClient;
    public Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> MockCollectionCalendarApiClient;
    public GetAllEarningsQueryResult Result;

    private GetAllEarningsQueryHandler _handler;
    private GetAllEarningsQuery _query;

    public GetAllEarningsQueryTestFixture(TestScenario scenario)
    {
        // Arrange
        MockApprenticeshipsApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        MockEarningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
        MockCollectionCalendarApiClient = new Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>>();

        Ukprn = Fixture.Create<long>();
        CollectionPeriod = 2;
        CollectionYear = 2425;

        var dataGenerator = new MockDataGenerator.MockDataGenerator();
        dataGenerator.GenerateData(scenario);

        LearningsResponse = dataGenerator.GetLearningsResponse;
        EarningsResponse = dataGenerator.GetFm36DataResponse;

        CollectionCalendarResponse = BuildCollectionCalendarResponse(LearningsResponse);
        SetupMocks(Ukprn, MockApprenticeshipsApiClient, LearningsResponse, MockEarningsApiClient, EarningsResponse, MockCollectionCalendarApiClient, CollectionCalendarResponse);

        _handler = new GetAllEarningsQueryHandler(MockApprenticeshipsApiClient.Object, MockEarningsApiClient.Object, MockCollectionCalendarApiClient.Object, Mock.Of<ILogger<GetAllEarningsQueryHandler>>());
        _query = new GetAllEarningsQuery(Ukprn, CollectionYear, CollectionPeriod);
    }

    public GetAcademicYearsResponse BuildCollectionCalendarResponse(GetLearningsResponse learningsResponse, bool apprenticeshipStartedInCurrentAcademicYear = true)
    {
        return new GetAcademicYearsResponse
        {
            AcademicYear = "2021",
            StartDate = new DateTime(2020, 8, 1),
            EndDate = new DateTime(2021, 7, 31)
        };
    }

    public void SetupMocks(
        long ukprn,
        Mock<ILearningApiClient<LearningApiConfiguration>> mockApprenticeshipsApiClient,
        GetLearningsResponse learningsResponse,
        Mock<IEarningsApiClient<EarningsApiConfiguration>> mockEarningsApiClient,
        GetFm36DataResponse earningsResponse,
        Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> mockCollectionCalendarApiClient,
        GetAcademicYearsResponse collectionCalendarResponse)
    {
        mockApprenticeshipsApiClient
            .Setup(x => x.Get<GetLearningsResponse>(It.Is<GetLearningsRequest>(r => r.Ukprn == ukprn)))
            .ReturnsAsync(learningsResponse);

        mockEarningsApiClient
            .Setup(x => x.Get<GetFm36DataResponse>(It.Is<GetFm36DataRequest>(r => r.Ukprn == ukprn)))
            .ReturnsAsync(earningsResponse);

        MockCollectionCalendarApiClient
            .Setup(x => x.Get<GetAcademicYearsResponse>(It.Is<GetAcademicYearByYearRequest>(y => y.GetUrl == $"academicyears/{CollectionYear}")))
            .ReturnsAsync(collectionCalendarResponse);
    }

    public async Task CallSubjectUnderTest()
    {
        // Act
        Result = await _handler.Handle(_query, CancellationToken.None);
    }

    public IEnumerable<(Episode Episode, EpisodePrice Price)> GetExpectedPriceEpisodesSplitByAcademicYear(List<Episode> apprenticeshipEpisodes)
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