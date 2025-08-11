using AutoFixture;
using SFA.DAS.LearnerData.Application.Fm36;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;

internal class MockDataGenerator
{
    private long _ukprn;
    private const string AdditionalPaymentTypeProviderIncentive = "ProviderIncentive";
    private const string AdditionalPaymentTypeEmployerIncentive = "EmployerIncentive";
    private const string AdditionalPaymentTypeLearningSupport = "LearningSupport";

    internal GetLearningsResponse GetLearningsResponse { get; private set; }
    internal GetFm36DataResponse GetFm36DataResponse { get; private set; }
    private Fixture Fixture { get; set; }

    internal MockDataGenerator()
    {
        Fixture = new Fixture();
    }

    private void InstantiateResponses()
    {
        _ukprn = Fixture.Create<long>();

        GetLearningsResponse = new GetLearningsResponse
        {
            Ukprn = _ukprn,
            Learnings = []
        };
        GetFm36DataResponse = new GetFm36DataResponse();
    }

    internal void GenerateData(TestScenario scenario)
    {
        InstantiateResponses();
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
    }

    private void AddSimpleApprenticeship()
    {
        var simpleApprenticeship =
            new Learning
            {
                Uln = Fixture.Create<int>().ToString(),
                Key = Guid.NewGuid(),
                Episodes =
                [
                    new SharedOuterApi.InnerApi.Responses.Learning.Episode
                        {
                            Key = Guid.NewGuid(),
                            TrainingCode = $"{Fixture.Create<int>()}    ",
                            Prices = new List<EpisodePrice>
                            {
                                new EpisodePrice
                                {
                                    Key = Guid.NewGuid(),
                                    StartDate = new DateTime(2020, 1, 1),
                                    EndDate = new DateTime(2021, 1, 1),
                                    TrainingPrice = 14000,
                                    EndPointAssessmentPrice = 1000,
                                    TotalPrice = 15000,
                                    FundingBandMaximum = 19000
                                }
                            }
                        }
                ],
                StartDate = new DateTime(2020, 1, 1),
                PlannedEndDate = new DateTime(2021, 1, 1),
                AgeAtStartOfApprenticeship = 18
            };

        var earningsApprenticeship = new Apprenticeship
        {
            Key = simpleApprenticeship.Key,
            Ukprn = _ukprn,
            FundingLineType = Fixture.Create<string>(),
            Episodes = new List<SharedOuterApi.InnerApi.Responses.Earnings.Episode>
                {
                    new SharedOuterApi.InnerApi.Responses.Earnings.Episode
                    {
                        Key = simpleApprenticeship.Episodes[0].Key,
                        NumberOfInstalments = 12,
                        CompletionPayment = 3000,
                        OnProgramTotal = 12000,
                        Instalments = new List<Instalment>
                        {
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 6, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(1920,6), InstalmentType = InstalmentType.Regular.ToString() },
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 7, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(1920,7), InstalmentType =InstalmentType.Regular.ToString() },
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 9, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(1920,9), InstalmentType = InstalmentType.Regular.ToString() },
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 10, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(1920, 10), InstalmentType = InstalmentType.Regular.ToString() },
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 11, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(1920, 11), InstalmentType = InstalmentType.Regular.ToString() },
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 12, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(1920, 12) , InstalmentType = InstalmentType.Regular.ToString()},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 1, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(2021, 1), InstalmentType = InstalmentType.Regular.ToString() },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 2, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(2021, 2), InstalmentType = InstalmentType.Regular.ToString() },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 3, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(2021, 3), InstalmentType = InstalmentType.Regular.ToString() },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 4, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(2021, 4), InstalmentType = InstalmentType.Regular.ToString() },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 5, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(2021, 5), InstalmentType = InstalmentType.Regular.ToString() }
                        },
                        AdditionalPayments = new List<AdditionalPayment>
                        {
                            new AdditionalPayment{ AcademicYear = 1920, DeliveryPeriod = 8, Amount = 500, AdditionalPaymentType = AdditionalPaymentTypeProviderIncentive, DueDate = new DateTime(2020, 3, 30) },
                            new AdditionalPayment{ AcademicYear = 1920, DeliveryPeriod = 8, Amount = 500, AdditionalPaymentType = AdditionalPaymentTypeEmployerIncentive, DueDate = new DateTime(2020, 3, 30) },

                            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 5, Amount = 500, AdditionalPaymentType = AdditionalPaymentTypeProviderIncentive, DueDate = new DateTime(2020, 12, 30) },
                            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 5, Amount = 500, AdditionalPaymentType = AdditionalPaymentTypeEmployerIncentive, DueDate = new DateTime(2020, 12, 30) },

                            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 3, Amount = 150, AdditionalPaymentType = AdditionalPaymentTypeLearningSupport, DueDate = new DateTime(2020, 10, 30) },
                            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 4, Amount = 150, AdditionalPaymentType = AdditionalPaymentTypeLearningSupport, DueDate = new DateTime(2020, 11, 30) },
                            new AdditionalPayment{ AcademicYear = 2021, DeliveryPeriod = 5, Amount = 150, AdditionalPaymentType = AdditionalPaymentTypeLearningSupport, DueDate = new DateTime(2020, 12, 30) },
                        }
                    }
                }
        };

        GetLearningsResponse.Learnings.Add(simpleApprenticeship);
        GetFm36DataResponse.Add(earningsApprenticeship);
    }

    private void AddApprenticeshipWithPriceChange()
    {
        var apprenticeshipWithAPriceChange =
            new Learning
            {
                Uln = Fixture.Create<int>().ToString(),
                Key = Guid.NewGuid(),
                Episodes = new List<SharedOuterApi.InnerApi.Responses.Learning.Episode>
                {
                        new SharedOuterApi.InnerApi.Responses.Learning.Episode
                        {
                            Key = Guid.NewGuid(),
                            TrainingCode = $"{Fixture.Create<int>()}    ",
                            Prices = new List<EpisodePrice>
                            {
                                new EpisodePrice
                                {
                                    Key = Guid.NewGuid(),
                                    StartDate = new DateTime(2020, 8, 1),
                                    EndDate = new DateTime(2021, 5, 2),
                                    TrainingPrice = 21000,
                                    EndPointAssessmentPrice = 1500,
                                    TotalPrice = 22500,
                                    FundingBandMaximum = 30000
                                },
                                new EpisodePrice
                                {
                                    Key = Guid.NewGuid(),
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

        var earnings = new Apprenticeship
        {
            Key = apprenticeshipWithAPriceChange.Key,
            Ukprn = _ukprn,
            FundingLineType = Fixture.Create<string>(),
            Episodes = new List<SharedOuterApi.InnerApi.Responses.Earnings.Episode>
                {
                    new SharedOuterApi.InnerApi.Responses.Earnings.Episode
                    {
                        Key = apprenticeshipWithAPriceChange.Episodes[0].Key,
                        NumberOfInstalments = 12,
                        CompletionPayment = 6000,
                        OnProgramTotal = 24000,
                        Instalments = new List<Instalment>
                        {
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 1, Amount = 1500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,1), InstalmentType = InstalmentType.Regular.ToString()},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 2, Amount = 1500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,2), InstalmentType = InstalmentType.Regular.ToString()},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 3, Amount = 1500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,3), InstalmentType = InstalmentType.Regular.ToString()},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 4, Amount = 1500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,4), InstalmentType = InstalmentType.Regular.ToString()},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 5, Amount = 1500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,5), InstalmentType = InstalmentType.Regular.ToString()},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 6, Amount = 1500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,6), InstalmentType = InstalmentType.Regular.ToString()},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 7, Amount = 1500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,7), InstalmentType = InstalmentType.Regular.ToString()},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 8, Amount = 1500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,8), InstalmentType = InstalmentType.Regular.ToString()},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 9, Amount = 1500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,9), InstalmentType = InstalmentType.Regular.ToString()},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 10, Amount = 3500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,10), InstalmentType = InstalmentType.Regular.ToString()},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 11, Amount = 3500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,11), InstalmentType = InstalmentType.Regular.ToString()},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 12, Amount = 3500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,12), InstalmentType = InstalmentType.Regular.ToString()}
                        },
                        AdditionalPayments = new List<AdditionalPayment>
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
                        }
                    }
                }
        };

        GetLearningsResponse.Learnings.Add(apprenticeshipWithAPriceChange);
        GetFm36DataResponse.Add(earnings);
    }
}
