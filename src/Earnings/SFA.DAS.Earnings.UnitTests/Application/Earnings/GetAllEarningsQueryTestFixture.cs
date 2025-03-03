using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Earnings.Application.Earnings;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.Interfaces;
using Apprenticeship = SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships.Apprenticeship;
using Episode = SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships.Episode;

namespace SFA.DAS.Earnings.UnitTests.Application.Earnings;

public class GetAllEarningsQueryTestFixture
{
        
    public readonly Fixture Fixture = new();
    public long Ukprn;
    public byte CollectionPeriod;
    public int CollectionYear;
    public GetApprenticeshipsResponse ApprenticeshipsResponse;
    public GetFm36DataResponse EarningsResponse;
    public GetAcademicYearsResponse CollectionCalendarResponse;
    public Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> MockApprenticeshipsApiClient;
    public Mock<IEarningsApiClient<EarningsApiConfiguration>> MockEarningsApiClient;
    public Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> MockCollectionCalendarApiClient;
    public GetAllEarningsQueryResult Result;

    private GetAllEarningsQueryHandler _handler;
    private GetAllEarningsQuery _query;

    public GetAllEarningsQueryTestFixture()
    {
        // Arrange
        MockApprenticeshipsApiClient = new Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>>();
        MockEarningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
        MockCollectionCalendarApiClient = new Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>>();

        Ukprn = Fixture.Create<long>();
        CollectionPeriod = 2;
        CollectionYear = 2425;
        ApprenticeshipsResponse = BuildApprenticeshipsResponse(Ukprn);
        EarningsResponse = BuildEarningsResponse(ApprenticeshipsResponse);
        CollectionCalendarResponse = BuildCollectionCalendarResponse(ApprenticeshipsResponse);
        SetupMocks(Ukprn, MockApprenticeshipsApiClient, ApprenticeshipsResponse, MockEarningsApiClient, EarningsResponse, MockCollectionCalendarApiClient, CollectionCalendarResponse);


        _handler = new GetAllEarningsQueryHandler(MockApprenticeshipsApiClient.Object, MockEarningsApiClient.Object, MockCollectionCalendarApiClient.Object, Mock.Of<ILogger<GetAllEarningsQueryHandler>>());
        _query = new GetAllEarningsQuery(Ukprn, CollectionYear, CollectionPeriod);
    }

    public GetApprenticeshipsResponse BuildApprenticeshipsResponse(long ukprn)
    {
        //Simple apprenticeship, spans an academic year boundary,
        // so we can use this to test that a new price episode is created in the fm36 when a new academic year starts
        var simpleApprenticeship =
            new Apprenticeship
            {
                Uln = Fixture.Create<int>().ToString(),
                Key = Guid.NewGuid(),
                Episodes = new List<Episode>
                {
                    new Episode
                    {
                        Key = Guid.NewGuid(),
                        TrainingCode = $"{Fixture.Create<int>()}    ",
                        Prices = new List<EpisodePrice>
                        {
                            new EpisodePrice
                            {
                                StartDate = new DateTime(2020, 1, 1),
                                EndDate = new DateTime(2021, 1, 1),
                                TrainingPrice = 14000,
                                EndPointAssessmentPrice = 1000,
                                TotalPrice = 15000,
                                FundingBandMaximum = 19000
                            }
                        }
                    }
                },
                StartDate = new DateTime(2020, 1, 1),
                PlannedEndDate = new DateTime(2021, 1, 1),
                AgeAtStartOfApprenticeship = 18
            };

        //Apprenticeship with a price change
        var apprenticeshipWithAPriceChange =
            new Apprenticeship
            {
                Uln = Fixture.Create<int>().ToString(),
                Key = Guid.NewGuid(),
                Episodes = new List<Episode>
                {
                    new Episode
                    {
                        Key = Guid.NewGuid(),
                        TrainingCode = $"{Fixture.Create<int>()}    ",
                        Prices = new List<EpisodePrice>
                        {
                            new EpisodePrice
                            {
                                StartDate = new DateTime(2020, 8, 1),
                                EndDate = new DateTime(2021, 5, 2),
                                TrainingPrice = 21000,
                                EndPointAssessmentPrice = 1500,
                                TotalPrice = 22500,
                                FundingBandMaximum = 30000
                            },
                            new EpisodePrice
                            {
                                StartDate = new DateTime(2021, 5, 3),
                                EndDate = new DateTime(2021, 7, 31),
                                TrainingPrice = 28500,
                                EndPointAssessmentPrice = 1500,
                                TotalPrice = 30000,
                                FundingBandMaximum = 30000
                            }
                        }
                    }
                },
                StartDate = new DateTime(2020, 8, 1),
                PlannedEndDate = new DateTime(2021, 7, 31),
                AgeAtStartOfApprenticeship = 19
            };

        var response = new GetApprenticeshipsResponse
        {
            Ukprn = ukprn,
            Apprenticeships = new List<Apprenticeship>
            {
                simpleApprenticeship,
                apprenticeshipWithAPriceChange
            },
            SimpleApprenticeship = simpleApprenticeship,
            ApprenticeshipWithAPriceChange = apprenticeshipWithAPriceChange
        };
        response.Ukprn = ukprn;
        response.Apprenticeships.ForEach(x => x.Uln = Fixture.Create<long>().ToString());
        return response;
    }

    public GetFm36DataResponse BuildEarningsResponse(GetApprenticeshipsResponse apprenticeshipsResponse)
    {
        const string additionalPaymentTypeProviderIncentive = "ProviderIncentive";
        const string additionalPaymentTypeEmployerIncentive = "EmployerIncentive";

        var response = new GetFm36DataResponse
        {
            new SharedOuterApi.InnerApi.Responses.Earnings.Apprenticeship
            {
                Key = apprenticeshipsResponse.Apprenticeships[0].Key,
                Ukprn = apprenticeshipsResponse.Ukprn,
                FundingLineType = Fixture.Create<string>(),
                Episodes = new List<SharedOuterApi.InnerApi.Responses.Earnings.Episode>
                {
                    new SharedOuterApi.InnerApi.Responses.Earnings.Episode
                    {
                        Key = apprenticeshipsResponse.Apprenticeships[0].Episodes[0].Key,
                        NumberOfInstalments = 12,
                        CompletionPayment = 3000,
                        OnProgramTotal = 12000,
                        Instalments = new List<Instalment>
                        {
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 6, Amount = 1000 },
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 7, Amount = 1000 },
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 8, Amount = 1000 },
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 9, Amount = 1000 },
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 10, Amount = 1000 },
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 11, Amount = 1000 },
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 12, Amount = 1000 },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 1, Amount = 1000 },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 2, Amount = 1000 },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 3, Amount = 1000 },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 4, Amount = 1000 },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 5, Amount = 1000 }
                        },
                        AdditionalPayments = new List<AdditionalPayment>
                        {
                            new AdditionalPayment{ AcademicYear = 1920, DeliveryPeriod = 6, Amount = 500, AdditionalPaymentType = additionalPaymentTypeProviderIncentive, DueDate = new DateTime(2020, 1, 1)},
                            new AdditionalPayment{ AcademicYear = 1920, DeliveryPeriod = 7, Amount = 500, AdditionalPaymentType = additionalPaymentTypeProviderIncentive, DueDate = new DateTime(2020, 2, 1)},
                            new AdditionalPayment{ AcademicYear = 1920, DeliveryPeriod = 8, Amount = 500, AdditionalPaymentType = additionalPaymentTypeProviderIncentive, DueDate = new DateTime(2020, 3, 1)},
                            new AdditionalPayment{ AcademicYear = 1920, DeliveryPeriod = 9, Amount = 500, AdditionalPaymentType = additionalPaymentTypeProviderIncentive, DueDate = new DateTime(2020, 4, 1)},
                            new AdditionalPayment{ AcademicYear = 1920, DeliveryPeriod = 10, Amount = 500, AdditionalPaymentType = additionalPaymentTypeProviderIncentive, DueDate = new DateTime(2020, 5, 1) },
                            new AdditionalPayment{ AcademicYear = 1920, DeliveryPeriod = 11, Amount = 500, AdditionalPaymentType = additionalPaymentTypeProviderIncentive, DueDate = new DateTime(2020, 6, 1) },
                            new AdditionalPayment{ AcademicYear = 1920, DeliveryPeriod = 12, Amount = 500, AdditionalPaymentType = additionalPaymentTypeProviderIncentive, DueDate = new DateTime(2020, 7, 1) },
                            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 1, Amount = 500, AdditionalPaymentType = additionalPaymentTypeProviderIncentive, DueDate = new DateTime(2020, 8, 1) },
                            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 2, Amount = 500, AdditionalPaymentType = additionalPaymentTypeProviderIncentive, DueDate = new DateTime(2020, 9, 1) },

                            new AdditionalPayment{ AcademicYear = 1920, DeliveryPeriod = 6, Amount = 500, AdditionalPaymentType = additionalPaymentTypeEmployerIncentive, DueDate = new DateTime(2020, 10, 1)},
                            new AdditionalPayment{ AcademicYear = 1920, DeliveryPeriod = 7, Amount = 500, AdditionalPaymentType = additionalPaymentTypeEmployerIncentive, DueDate = new DateTime(2020, 11, 1)}
                        }
                    }
                }
            },
            new SharedOuterApi.InnerApi.Responses.Earnings.Apprenticeship
            {
                Key = apprenticeshipsResponse.Apprenticeships[1].Key,
                Ukprn = apprenticeshipsResponse.Ukprn,
                FundingLineType = Fixture.Create<string>(),
                Episodes = new List<SharedOuterApi.InnerApi.Responses.Earnings.Episode>
                {
                    new SharedOuterApi.InnerApi.Responses.Earnings.Episode
                    {
                        Key = apprenticeshipsResponse.Apprenticeships[1].Episodes[0].Key,
                        NumberOfInstalments = 12,
                        CompletionPayment = 6000,
                        OnProgramTotal = 24000,
                        Instalments = new List<Instalment>
                        {
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 1, Amount = 1875 },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 2, Amount = 1875 },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 3, Amount = 1875 },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 4, Amount = 1875 },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 5, Amount = 1875 },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 6, Amount = 1875 },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 7, Amount = 1875 },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 8, Amount = 1875 },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 9, Amount = 1875 },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 10, Amount = 4375 },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 11, Amount = 4375 },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 12, Amount = 4375 }
                        },
                        AdditionalPayments = new List<AdditionalPayment>
                        {
                            new AdditionalPayment{ AcademicYear = 2020, DeliveryPeriod = 12, Amount = 1875, AdditionalPaymentType = additionalPaymentTypeProviderIncentive, DueDate = new DateTime(2020, 7, 1)},
                        }
                    }
                }
            }
        };
        response.ForEach(x => x.Ukprn = apprenticeshipsResponse.Ukprn);
        return response;
    }

    public GetAcademicYearsResponse BuildCollectionCalendarResponse(GetApprenticeshipsResponse apprenticeshipsResponse, bool apprenticeshipStartedInCurrentAcademicYear = true)
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
        Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> mockApprenticeshipsApiClient,
        GetApprenticeshipsResponse apprenticeshipsResponse,
        Mock<IEarningsApiClient<EarningsApiConfiguration>> mockEarningsApiClient,
        GetFm36DataResponse earningsResponse,
        Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> mockCollectionCalendarApiClient,
        GetAcademicYearsResponse collectionCalendarResponse)
    {
        mockApprenticeshipsApiClient
            .Setup(x => x.Get<GetApprenticeshipsResponse>(It.Is<GetApprenticeshipsRequest>(r => r.Ukprn == ukprn)))
            .ReturnsAsync(apprenticeshipsResponse);

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

public static class GetAllEarningsQueryTestFixtureExtensions
{
    public static void EditApprenticeshipResponse(this GetAllEarningsQueryTestFixture fixture, int index, Action<Apprenticeship> editAction)
    {
        if (index < 0 || index >= fixture.ApprenticeshipsResponse.Apprenticeships.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        }

        var apprenticeship = fixture.ApprenticeshipsResponse.Apprenticeships[index];
        editAction(apprenticeship);
    }

}