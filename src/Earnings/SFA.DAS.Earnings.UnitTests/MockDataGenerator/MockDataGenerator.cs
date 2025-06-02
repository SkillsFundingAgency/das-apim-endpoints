﻿using AutoFixture;
using SFA.DAS.Earnings.UnitTests.Application.Earnings;
using SFA.DAS.Earnings.UnitTests.Application.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using Apprenticeship = SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships.Apprenticeship;

namespace SFA.DAS.Earnings.UnitTests.MockDataGenerator
{
    public class MockDataGenerator
    {
        private long _ukprn;
        private const string AdditionalPaymentTypeProviderIncentive = "ProviderIncentive";
        private const string AdditionalPaymentTypeEmployerIncentive = "EmployerIncentive";
        private const string AdditionalPaymentTypeLearningSupport = "LearningSupport";

        public GetApprenticeshipsResponse GetApprenticeshipsResponse { get; private set; }
        public GetFm36DataResponse GetFm36DataResponse { get; private set; }
        private Fixture Fixture { get; set; }

        public MockDataGenerator()
        {
            Fixture = new Fixture();
        }

        private void InstantiateResponses()
        {
            _ukprn = Fixture.Create<long>();

            GetApprenticeshipsResponse = new GetApprenticeshipsResponse
            {
                Ukprn = _ukprn,
                Apprenticeships = []
            };
            GetFm36DataResponse = new GetFm36DataResponse();
        }

        public void GenerateData(TestScenario scenario)
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
                new Apprenticeship
                {
                    Uln = Fixture.Create<int>().ToString(),
                    Key = Guid.NewGuid(),
                    Episodes =
                    [
                        new SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships.Episode
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

            var earningsApprenticeship = new SharedOuterApi.InnerApi.Responses.Earnings.Apprenticeship
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
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 6, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(1920,6) },
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 7, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(1920,7) },
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 8, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(1920,8) },
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 9, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(1920,9) },
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 10, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(1920, 10) },
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 11, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(1920, 11) },
                            new Instalment{ AcademicYear = 1920, DeliveryPeriod = 12, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(1920, 12) },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 1, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(2021, 1) },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 2, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(2021, 2) },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 3, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(2021, 3) },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 4, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(2021, 4) },
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 5, Amount = 1000, EpisodePriceKey = simpleApprenticeship.GetEpisodePriceKey(2021, 5) }
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

            GetApprenticeshipsResponse.Apprenticeships.Add(simpleApprenticeship);
            GetFm36DataResponse.Add(earningsApprenticeship);
        }

        private void AddApprenticeshipWithPriceChange()
        {
            var apprenticeshipWithAPriceChange =
                new Apprenticeship
                {
                    Uln = Fixture.Create<int>().ToString(),
                    Key = Guid.NewGuid(),
                    Episodes = new List<SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships.Episode>
                    {
                        new SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships.Episode
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

            var earnings = new SharedOuterApi.InnerApi.Responses.Earnings.Apprenticeship
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
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 1, Amount = 1500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,1)},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 2, Amount = 1500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,2)},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 3, Amount = 1500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,3)},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 4, Amount = 1500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,4)},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 5, Amount = 1500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,5)},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 6, Amount = 1500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,6)},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 7, Amount = 1500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,7)},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 8, Amount = 1500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,8)},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 9, Amount = 1500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,9)},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 10, Amount = 3500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,10)},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 11, Amount = 3500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,11)},
                            new Instalment{ AcademicYear = 2021, DeliveryPeriod = 12, Amount = 3500 , EpisodePriceKey = apprenticeshipWithAPriceChange.GetEpisodePriceKey(2021,12)}
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

            GetApprenticeshipsResponse.Apprenticeships.Add(apprenticeshipWithAPriceChange);
            GetFm36DataResponse.Add(earnings);
        }
    }
}
