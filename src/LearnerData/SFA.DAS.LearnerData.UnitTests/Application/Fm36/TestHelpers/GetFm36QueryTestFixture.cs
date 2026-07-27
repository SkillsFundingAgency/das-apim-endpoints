using AutoFixture;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.LearnerData.Application.Fm36;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.LearnerData.Shared;
using SFA.DAS.LearnerData.TestHelpers;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using EarningEpisode = SFA.DAS.LearnerData.Responses.EarningsInner.Episode;
using LearningEpisode = SFA.DAS.LearnerData.Responses.LearningInner.Episode;

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
    internal GetAcademicYearsResponse CollectionCalendarResponse => _collectionYearResponses[CollectionYear];
    internal List<UpdateLearnerRequest> SldLearnerData => _fm36TestContext.SldLearnerData;
    internal Mock<ILearningApiClient<LearningApiConfiguration>> MockApprenticeshipsApiClient = new();
    internal Mock<IEarningsApiClient<EarningsApiConfiguration>> MockEarningsApiClient = new();
    internal Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> MockCollectionCalendarApiClient = new();
    internal Mock<ILearnerDataCacheService> MockDistributedCache = new();
    internal GetFm36Result Result;

    private GetFm36QueryHandler _handler;
    private Fm36TestContext _fm36TestContext;
    private Dictionary<int,GetAcademicYearsResponse> _collectionYearResponses;

    internal GetFm36QueryTestFixture(TestScenario scenario): this(scenario, null)
    {
        
    }

    internal GetFm36QueryTestFixture(TestScenario scenario, Action<Fm36TestContext>? configure)
    {
        _collectionYearResponses = new Dictionary<int, GetAcademicYearsResponse>();
        for (var year = 2015; year <= 2030; year++)
        {
            var collectionCalendarResponse = BuildCollectionCalendarResponse(year);
            _collectionYearResponses.Add(int.Parse(collectionCalendarResponse.AcademicYear), collectionCalendarResponse);
        }

        _fm36TestContext = new Fm36TestContext();

        Ukprn = Fixture.Create<long>();
        CollectionPeriod = 2;
        CollectionYear = 2425;

        GenerateData(scenario);

        if (configure != null)
            configure(_fm36TestContext);

        if (scenario != TestScenario.NoData)
        {
            _fm36TestContext.Build();
        }

        SetupMocks(Ukprn);

        _handler = new GetFm36QueryHandler(MockApprenticeshipsApiClient.Object, MockEarningsApiClient.Object, MockCollectionCalendarApiClient.Object, MockDistributedCache.Object, Mock.Of<ILogger<GetFm36QueryHandler>>());
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
            case TestScenario.ApprenticeshipWithEnglish:
                AddApprenticeshipWithEnglish();
                break;
            case TestScenario.AllData:
                AddSimpleApprenticeship();
                AddApprenticeshipWithPriceChange();
                break;
            case TestScenario.LearningSupportComplexScenario:
                AddLearningSupportComplexScenario();
                break;
            default:
                throw new ArgumentOutOfRangeException();
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

    private void AddApprenticeshipWithEnglish()
    {
        var testLearner = DefaultLearner.CreateNew;
        testLearner.Ukprn = (int)Ukprn;
        testLearner.FundingBandMax = 19000;
        testLearner.ClearProgrammes();
        testLearner.AddProgramme(
            ageAtStart: 18,
            startDate: new DateTime(2020, 1, 1),
            endDate: new DateTime(2021, 1, 31),
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

        testLearner.AddEnglishAndMathsDelivery();

        _fm36TestContext.TestLearners.Add(testLearner);
    }

    private void AddLearningSupportComplexScenario()
    {
        var testLearner = DefaultLearner.CreateNew;
        testLearner.Ukprn = (int)Ukprn;
        testLearner.FundingBandMax = 19000;
        testLearner.ClearProgrammes();

        testLearner.AddProgramme(
            ageAtStart: 18,
            startDate: new DateTime(2020, 1, 1),
            endDate: new DateTime(2021, 1, 31),
            trainingPrice: 14000,
            endpointAssessmentPrice: 1000);

        testLearner.AdditionalPayments = new List<AdditionalPayment>
        {
            // Onprogramme additional payments
            new AdditionalPayment{ AcademicYear = 1920, DeliveryPeriod = 8, Amount = 500, AdditionalPaymentType = AdditionalPaymentTypeProviderIncentive, DueDate = new DateTime(2020, 3, 30) },
            new AdditionalPayment{ AcademicYear = 1920, DeliveryPeriod = 8, Amount = 500, AdditionalPaymentType = AdditionalPaymentTypeEmployerIncentive, DueDate = new DateTime(2020, 3, 30) },

            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 5, Amount = 500, AdditionalPaymentType = AdditionalPaymentTypeProviderIncentive, DueDate = new DateTime(2020, 12, 30) },
            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 5, Amount = 500, AdditionalPaymentType = AdditionalPaymentTypeEmployerIncentive, DueDate = new DateTime(2020, 12, 30) },

            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 3, Amount = 150, AdditionalPaymentType = AdditionalPaymentTypeLearningSupport, DueDate = new DateTime(2020, 10, 30) },
            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 4, Amount = 150, AdditionalPaymentType = AdditionalPaymentTypeLearningSupport, DueDate = new DateTime(2020, 11, 30) },
            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 5, Amount = 150, AdditionalPaymentType = AdditionalPaymentTypeLearningSupport, DueDate = new DateTime(2020, 12, 30) },

            // English and Maths additional payments
            // (note limitation of the test mocking this goes here, but it works because in earnings all learning support gets stored together)
            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 7, Amount = 150, AdditionalPaymentType = AdditionalPaymentTypeLearningSupport, DueDate = new DateTime(2021, 02, 28) },
            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 8, Amount = 150, AdditionalPaymentType = AdditionalPaymentTypeLearningSupport, DueDate = new DateTime(2021, 03, 31) },
            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 9, Amount = 150, AdditionalPaymentType = AdditionalPaymentTypeLearningSupport, DueDate = new DateTime(2021, 04, 30) },
        };

        testLearner.AddEnglishAndMathsDelivery(
            aimSequenceNumber:2,
            learnAimRef: "ENG001",
            course: "English",
            startDate: new DateTime(2020, 9, 1),
            endDate: new DateTime(2021, 1, 31),
            amount: 1500);

        testLearner.AddEnglishAndMathsDelivery(
            aimSequenceNumber: 3,
            learnAimRef: "MAT001",
            course: "Maths",
            startDate: new DateTime(2020, 12, 1),
            endDate: new DateTime(2021, 4, 30),
            amount: 1200,
            learningSupports: new List<LearningSupport>
                {
                    new LearningSupport{ StartDate = new DateTime(2021, 2, 1), EndDate = new DateTime(2021, 04, 30) },
                }
            );

        _fm36TestContext.TestLearners.Add(testLearner);
    }

    internal GetAcademicYearsResponse BuildCollectionCalendarResponse(int year)
    {
        var yearLastTwoDigits = (year % 100);

        var academicYear = $"{yearLastTwoDigits}{yearLastTwoDigits+1}";

        return new GetAcademicYearsResponse
        {
            AcademicYear = academicYear,
            StartDate = new DateTime(year, 8, 1),
            EndDate = new DateTime(year + 1, 7, 31)
        };
    }

    internal void SetupMocks(long ukprn)
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

        foreach (var collectionCalendarResponse in _collectionYearResponses.Values)
        {
            MockCollectionCalendarApiClient
                .Setup(x => x.Get<GetAcademicYearsResponse>(It.Is<GetAcademicYearByYearRequest>(y => y.GetUrl == $"academicyears/{collectionCalendarResponse.AcademicYear}")))
                .ReturnsAsync(collectionCalendarResponse);
        }

        var learners = _fm36TestContext.SldLearnerData.Select(x => x.Learner.Uln.ToString());
        MockDistributedCache.Setup(x => x.GetLearners<UpdateLearnerRequest>(ukprn, learners, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fm36TestContext.SldLearnerData);
    }

    public enum QueryType
    {
        Paged,
        Unpaged
    }

    internal async Task CallSubjectUnderTest(QueryType queryType = QueryType.Unpaged, int? collectionYear = null)
    {
        if (collectionYear.HasValue)
        {
            CollectionYear = collectionYear.Value;
        }

        int? page = null;
        int? pageSize = null;

        if (queryType == QueryType.Paged)
        {
            page = 1;
            pageSize = 1;
        }

        var query = new GetFm36Query(Ukprn, CollectionYear, CollectionPeriod, page, pageSize);

        // Act
        Result = await _handler.Handle(query, CancellationToken.None);
    }

    internal IEnumerable<(LearningEpisode Episode, EpisodePrice Price)> GetExpectedPriceEpisodesSplitByAcademicYear(List<LearningEpisode> apprenticeshipEpisodes)
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

                yield return new ValueTuple<LearningEpisode, EpisodePrice>(episodePrice.Episode, price);
            }
            else
            {
                yield return episodePrice;
            }
        }
    }

    internal LearningDelivery GetLearningDelivery(TestScenario testScenario)
    {
        Result.Should().NotBeNull();
        var learning = UnpagedLearningsResponse.Single();

        LearningDelivery? learningDelivery;
        if (testScenario == TestScenario.ApprenticeshipWithEnglish)
        {
            learningDelivery = Result!.Items?.SingleOrDefault(learner =>
                learner.ULN.ToString() == learning.Uln
                )?.LearningDeliveries.SingleOrDefault(delivery => delivery.AimSeqNumber == 2);
        }
        else
        {
            learningDelivery = Result!.Items?.SingleOrDefault(learner => learner.ULN.ToString() == learning.Uln).LearningDeliveries.SingleOrDefault();
        }

        learningDelivery.Should().NotBeNull();

        return learningDelivery;
    }

    internal LearningDelivery GetLearningDeliveryByAimSequenceNumber(int aimSequenceNumber)
    {
        Result.Should().NotBeNull();
        var learning = UnpagedLearningsResponse.Single();
        var fm36Result = Result!.Items!.Single(learner => learner.ULN.ToString() == learning.Uln);
        return fm36Result.LearningDeliveries.Single(delivery => delivery.AimSeqNumber == aimSequenceNumber);
    }

    internal EarningEpisode GetEarningEpisode()
    {
        var earningApprenticeship = EarningsResponse.Apprenticeships.First();
        var earningEpisode = earningApprenticeship.Episodes.Single();
        return earningEpisode;
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
