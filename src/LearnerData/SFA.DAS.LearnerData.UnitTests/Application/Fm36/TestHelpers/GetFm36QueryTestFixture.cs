using AutoFixture;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.LearnerData.Application.Fm36;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.TestHelpers;
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
    private const string AdditionalPaymentTypeProviderIncentive = "ProviderIncentive";
    private const string AdditionalPaymentTypeEmployerIncentive = "EmployerIncentive";
    private const string AdditionalPaymentTypeLearningSupport = "LearningSupport";

    internal readonly Fixture Fixture = new();
    internal long Ukprn;
    internal byte CollectionPeriod;
    internal int CollectionYear;
    internal List<Learning> UnpagedLearningsResponse => _fm36TestContext.LearningInnerApiResponse;
    internal GetFm36DataResponse EarningsResponse => _fm36TestContext.EarningsInnerApiResponse;
    internal GetAcademicYearsResponse CollectionCalendarResponse;
    internal List<UpdateLearnerRequest> SldLearnerData => _fm36TestContext.SldLearnerData;
    internal Mock<ILearningApiClient<LearningApiConfiguration>> MockApprenticeshipsApiClient;
    internal Mock<IEarningsApiClient<EarningsApiConfiguration>> MockEarningsApiClient;
    internal Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> MockCollectionCalendarApiClient;
    internal Mock<IDistributedCache> MockDistributedCache;
    internal GetFm36Result Result;

    private GetFm36QueryHandler _handler;
    private GetFm36Query _query;
    private Fm36TestContext _fm36TestContext;

    internal GetFm36QueryTestFixture(TestScenario scenario)
    {
        // Arrange
        _fm36TestContext = new Fm36TestContext();
        MockApprenticeshipsApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        MockEarningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
        MockCollectionCalendarApiClient = new Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>>();
        MockDistributedCache = new Mock<IDistributedCache>();

        Ukprn = Fixture.Create<long>();
        CollectionPeriod = 2;
        CollectionYear = 2425;

        GenerateData(scenario);

        CollectionCalendarResponse = BuildCollectionCalendarResponse();
        SetupMocks(Ukprn, CollectionCalendarResponse);

        _handler = new GetFm36QueryHandler(MockApprenticeshipsApiClient.Object, MockEarningsApiClient.Object, MockCollectionCalendarApiClient.Object, MockDistributedCache.Object, Mock.Of<ILogger<GetFm36QueryHandler>>());
        _query = new GetFm36Query(Ukprn, CollectionYear, CollectionPeriod, null, null);
    }

    internal void GenerateData(TestScenario scenario)
    {
        switch (scenario)
        {
            case TestScenario.NoData:
                break;
            case TestScenario.SimpleApprenticeship:
                AddSimpleApprenticeship();
                break;
            case TestScenario.ApprenticeshipWithPriceChange:
                AddApprenticeshipWithPriceChange();
                break;
            case TestScenario.AllData:
                AddSimpleApprenticeship();
                AddApprenticeshipWithPriceChange();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (scenario != TestScenario.NoData)
        {
            _fm36TestContext.Build();
        }

    }

    private void AddSimpleApprenticeship()
    {
        var testLearner = DefaultLearner.CreateNew;
        testLearner.Ukprn = (int)Ukprn;
        testLearner.FundingBandMax = 19000;
        testLearner.ClearProgrammes();
        testLearner.AddProgramme(
            ageAtStart: 18,
            startDate: new DateTime(2020, 1, 1),
            endDate: new DateTime(2021, 1, 1),
            trainingPrice: 14000,
            endpointAssessmentPrice: 1000);

        testLearner.AdditionalPayments = new List<AdditionalPayment>
        {
            new AdditionalPayment{ AcademicYear = 1920, DeliveryPeriod = 8, Amount = 500, AdditionalPaymentType = AdditionalPaymentTypeProviderIncentive, DueDate = new DateTime(2020, 3, 30) },
            new AdditionalPayment{ AcademicYear = 1920, DeliveryPeriod = 8, Amount = 500, AdditionalPaymentType = AdditionalPaymentTypeEmployerIncentive, DueDate = new DateTime(2020, 3, 30) },

            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 5, Amount = 500, AdditionalPaymentType = AdditionalPaymentTypeProviderIncentive, DueDate = new DateTime(2020, 12, 30) },
            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 5, Amount = 500, AdditionalPaymentType = AdditionalPaymentTypeEmployerIncentive, DueDate = new DateTime(2020, 12, 30) },

            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 3, Amount = 150, AdditionalPaymentType = AdditionalPaymentTypeLearningSupport, DueDate = new DateTime(2020, 10, 30) },
            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 4, Amount = 150, AdditionalPaymentType = AdditionalPaymentTypeLearningSupport, DueDate = new DateTime(2020, 11, 30) },
            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 5, Amount = 150, AdditionalPaymentType = AdditionalPaymentTypeLearningSupport, DueDate = new DateTime(2020, 12, 30) },
        };

        _fm36TestContext.TestLearners.Add(testLearner);

    }

    private void AddApprenticeshipWithPriceChange()
    {
        var testLearner = DefaultLearner.CreateNew;
        testLearner.Ukprn = (int)Ukprn;
        testLearner.FundingBandMax = 30000;
        testLearner.ClearProgrammes();

        var costs = new List<CostDetails> { new CostDetails
            {
                FromDate = new DateTime(2020, 8, 1),
                TrainingPrice = 21000,
                EpaoPrice = 1500
            },
            new CostDetails
            {
                FromDate = new DateTime(2021, 5, 3),
                TrainingPrice = 28500,
                EpaoPrice = 1500
            }
        };

        testLearner.AddProgramme(
            ageAtStart: 19,
            startDate: new DateTime(2020, 8, 1),
            endDate: new DateTime(2021, 7, 31),
            costs: costs);

        testLearner.AdditionalPayments = new List<AdditionalPayment>
        {
            //Provider incentives
            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 3, Amount = 500, AdditionalPaymentType = AdditionalPaymentTypeProviderIncentive, DueDate = new DateTime(2020, 10, 29)},
            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 12, Amount = 500, AdditionalPaymentType = AdditionalPaymentTypeProviderIncentive, DueDate = new DateTime(2021, 7, 31)},
                            
            //Employer incentives
            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 3, Amount = 500, AdditionalPaymentType = AdditionalPaymentTypeEmployerIncentive, DueDate = new DateTime(2020, 10, 29)},
            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 12, Amount = 500, AdditionalPaymentType = AdditionalPaymentTypeEmployerIncentive, DueDate = new DateTime(2021, 7, 31)},

            //Learning support
            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 9, Amount = 150, AdditionalPaymentType = AdditionalPaymentTypeLearningSupport, DueDate = new DateTime(2021, 4, 30)},
            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 10, Amount = 150, AdditionalPaymentType = AdditionalPaymentTypeLearningSupport, DueDate = new DateTime(2021, 5, 31)},
            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 11, Amount = 150, AdditionalPaymentType = AdditionalPaymentTypeLearningSupport, DueDate = new DateTime(2021, 6, 30)}
        };

        _fm36TestContext.TestLearners.Add(testLearner);

    }

    internal GetAcademicYearsResponse BuildCollectionCalendarResponse()
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
        GetAcademicYearsResponse collectionCalendarResponse)
    {
        MockApprenticeshipsApiClient
            .Setup(x => x.Get<List<Learning>>(It.Is<GetLearningsRequest>(r => r.Ukprn == ukprn)))
            .ReturnsAsync(_fm36TestContext.LearningInnerApiResponse);

        MockApprenticeshipsApiClient
            .Setup(x => x.Get<GetPagedLearnersFromLearningInner>(It.Is<GetLearningsRequest>(r => r.Ukprn == ukprn)))
            .ReturnsAsync(new GetPagedLearnersFromLearningInner { Items = _fm36TestContext.LearningInnerApiResponse, Page = 1, PageSize = _fm36TestContext.LearningInnerApiResponse.Count, TotalItems = _fm36TestContext.LearningInnerApiResponse.Count });


        var response = new ApiResponse<GetFm36DataResponse>(_fm36TestContext.EarningsInnerApiResponse, System.Net.HttpStatusCode.OK, string.Empty);
        MockEarningsApiClient
            .Setup(x => x.PostWithResponseCode<GetFm36DataResponse>(
                It.Is<PostGetFm36DataRequest>(r => r.Ukprn == ukprn), It.IsAny<bool>())).ReturnsAsync(response);

        MockCollectionCalendarApiClient
            .Setup(x => x.Get<GetAcademicYearsResponse>(It.Is<GetAcademicYearByYearRequest>(y => y.GetUrl == $"academicyears/{CollectionYear}")))
            .ReturnsAsync(collectionCalendarResponse);

        foreach (var learner in _fm36TestContext.SldLearnerData)
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
