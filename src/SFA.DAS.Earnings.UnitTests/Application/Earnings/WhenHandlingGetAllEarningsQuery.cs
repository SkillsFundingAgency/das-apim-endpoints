using AutoFixture;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using FluentAssertions;
using Moq;
using SFA.DAS.Apprenticeships.Types;
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

namespace SFA.DAS.Earnings.UnitTests.Application.Earnings
{
    public class WhenHandlingGetAllEarningsQuery
    {
        private readonly Fixture _fixture = new();
        private GetAllEarningsQueryResult _result;
        private long _ukprn;
        private GetApprenticeshipsResponse _apprenticeshipsResponse;
        private GetFm36DataResponse _earningsResponse;
        private GetAcademicYearsResponse _collectionCalendarResponse;
        private Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> _mockApprenticeshipsApiClient;
        private Mock<IEarningsApiClient<EarningsApiConfiguration>> _mockEarningsApiClient;
        private Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> _mockCollectionCalendarApiClient;
        private GetAllEarningsQueryHandler _handler;
        private GetAllEarningsQuery _query;

        [SetUp]
        public async Task Setup()
        {
            // Arrange
            _mockApprenticeshipsApiClient = new Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>>();
            _mockEarningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
            _mockCollectionCalendarApiClient = new Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>>();

            _ukprn = _fixture.Create<long>();
            _apprenticeshipsResponse = BuildApprenticeshipsResponse(_ukprn);
            _earningsResponse = BuildEarningsResponse(_apprenticeshipsResponse);
            _collectionCalendarResponse = BuildCollectionCalendarResponse(_apprenticeshipsResponse);
            SetupMocks(_ukprn, _mockApprenticeshipsApiClient, _apprenticeshipsResponse, _mockEarningsApiClient, _earningsResponse, _mockCollectionCalendarApiClient, _collectionCalendarResponse);


            _handler = new GetAllEarningsQueryHandler(_mockApprenticeshipsApiClient.Object, _mockEarningsApiClient.Object, _mockCollectionCalendarApiClient.Object);
            _query = new GetAllEarningsQuery { Ukprn = _ukprn };
        }

        [Test]
        public async Task ThenCallsApprenticeshipsApi()
        {
            // Act
            _result = await _handler.Handle(_query, CancellationToken.None);

            //Assert
            _mockApprenticeshipsApiClient.Verify(x => x.Get<GetApprenticeshipsResponse>(It.Is<GetApprenticeshipsRequest>(r => r.Ukprn == _ukprn)), Times.Once);
        }

        [Test]
        public async Task ThenCallsEarningsApi()
        {
            // Act
            _result = await _handler.Handle(_query, CancellationToken.None);

            //Assert
            _mockEarningsApiClient.Verify(x => x.Get<GetFm36DataResponse>(It.Is<GetFm36DataRequest>(r => r.Ukprn == _ukprn)), Times.Once);
        }

        [Test]
        public async Task ThenReturnsFM36LearnerIdentifiers()
        {
            // Act
            _result = await _handler.Handle(_query, CancellationToken.None);

            // Assert
            _result.Should().NotBeNull();

            foreach (var apprenticeship in _apprenticeshipsResponse.Apprenticeships)
            {
                _result.FM36Learners.Should().Contain(learner => learner.ULN == long.Parse(apprenticeship.Uln) && learner.LearnRefNumber == "9999999999");
            }
        }

        [Test]
        public async Task ThenReturnsPriceEpisodeValues()
        {
            // Act
            _result = await _handler.Handle(_query, CancellationToken.None);

            // Assert
            _result.Should().NotBeNull();
            
            foreach (var apprenticeship in _apprenticeshipsResponse.Apprenticeships)
            {
                var fm36Learner = _result.FM36Learners.SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));
                foreach (var apprenticeshipEpisode in apprenticeship.Episodes)
                {
                    foreach (var apprenticeshipEpisodePrice in apprenticeshipEpisode.Prices)
                    {
                        fm36Learner.PriceEpisodes.Should().Contain(episode => 
                            episode.PriceEpisodeIdentifier == $"25-{apprenticeshipEpisode.TrainingCode}-{apprenticeshipEpisodePrice.StartDate:dd/MM/yyyy}"
                            && episode.PriceEpisodeValues.EpisodeStartDate == apprenticeshipEpisodePrice.StartDate);
                    }
                }
            }
        }

        [Test]
        public async Task ThenAPriceEpisodeIsCreatedForEachPrice()
        {
            // Act
            _result = await _handler.Handle(_query, CancellationToken.None);

            // Assert
            _result.Should().NotBeNull();

            foreach (var apprenticeship in _apprenticeshipsResponse.Apprenticeships)
            {
                var fm36Learner = _result.FM36Learners
                    .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

                fm36Learner.PriceEpisodes.Count.Should().Be(apprenticeship.Episodes.SelectMany(episode => episode.Prices).Count());
            }
        }

        [Test]
        public async Task ThenALearningDeliveryIsCreatedForEachApprenticeship()
        {
            // Act
            _result = await _handler.Handle(_query, CancellationToken.None);

            // Assert
            _result.Should().NotBeNull();

            _result.FM36Learners.Length.Should().Be(_apprenticeshipsResponse.Apprenticeships.Count);
            _result.FM36Learners.SelectMany(learner => learner.LearningDeliveries).Count().Should().Be(_apprenticeshipsResponse.Apprenticeships.Count);
        }

        [Test]
        public async Task ThenReturnsLearningDeliveryValuesForEachApprenticeship()
        {
            // Act
            _result = await _handler.Handle(_query, CancellationToken.None);

            // Assert
            _result.Should().NotBeNull();

            foreach (var apprenticeship in _apprenticeshipsResponse.Apprenticeships)
            {
                var expectedPriceEpisodeStartDate = apprenticeship.StartDate > _collectionCalendarResponse.StartDate ? apprenticeship.StartDate : _collectionCalendarResponse.StartDate;
                var expectedPriceEpisodeEndDate = apprenticeship.PlannedEndDate < _collectionCalendarResponse.EndDate ? apprenticeship.PlannedEndDate : _collectionCalendarResponse.EndDate;
                var earningEpisode = _earningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();

                var learningDelivery = _result.FM36Learners.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
                learningDelivery.Should().NotBeNull();
                learningDelivery.AimSeqNumber.Should().Be(1);
                learningDelivery.LearningDeliveryValues.ActualDaysIL.Should().Be(0);
                learningDelivery.LearningDeliveryValues.ActualNumInstalm.Should().BeNull();
                learningDelivery.LearningDeliveryValues.AdjStartDate.Should().Be(apprenticeship.StartDate);
                learningDelivery.LearningDeliveryValues.AgeAtProgStart.Should().Be(apprenticeship.AgeAtStartOfApprenticeship);
                learningDelivery.LearningDeliveryValues.AppAdjLearnStartDate.Should().Be(apprenticeship.StartDate);
                learningDelivery.LearningDeliveryValues.ApplicCompDate.Should().Be(new DateTime(9999, 9, 9));
                learningDelivery.LearningDeliveryValues.CombinedAdjProp.Should().Be(1);
                learningDelivery.LearningDeliveryValues.Completed.Should().BeFalse();
                learningDelivery.LearningDeliveryValues.FirstIncentiveThresholdDate.Should().BeNull();
                learningDelivery.LearningDeliveryValues.FundStart.Should().BeTrue();
                learningDelivery.LearningDeliveryValues.LDApplic1618FrameworkUpliftTotalActEarnings.Should().Be(0);
                learningDelivery.LearningDeliveryValues.LearnAimRef.Should().Be("ZPROG001");
                learningDelivery.LearningDeliveryValues.LearnStartDate.Should().Be(apprenticeship.StartDate);
                learningDelivery.LearningDeliveryValues.LearnDel1618AtStart.Should().Be(apprenticeship.AgeAtStartOfApprenticeship < 19);
                learningDelivery.LearningDeliveryValues.LearnDelAppAccDaysIL.Should().Be((expectedPriceEpisodeEndDate - apprenticeship.StartDate).Days);
                learningDelivery.LearningDeliveryValues.LearnDelApplicDisadvAmount.Should().Be(0);
                learningDelivery.LearningDeliveryValues.LearnDelApplicEmp1618Incentive.Should().Be(0);
                learningDelivery.LearningDeliveryValues.LearnDelApplicEmpDate.Should().BeNull(); //TODO
                learningDelivery.LearningDeliveryValues.LearnDelApplicProv1618FrameworkUplift.Should().Be(0);
                learningDelivery.LearningDeliveryValues.LearnDelApplicProv1618Incentive.Should().Be(0);
                learningDelivery.LearningDeliveryValues.LearnDelAppPrevAccDaysIL.Should().Be((expectedPriceEpisodeEndDate - expectedPriceEpisodeStartDate).Days);
                learningDelivery.LearningDeliveryValues.LearnDelDisadAmount.Should().Be(0);
                learningDelivery.LearningDeliveryValues.LearnDelEligDisadvPayment.Should().BeFalse();
                learningDelivery.LearningDeliveryValues.LearnDelEmpIdFirstAdditionalPaymentThreshold.Should().BeNull();
                learningDelivery.LearningDeliveryValues.LearnDelEmpIdSecondAdditionalPaymentThreshold.Should().BeNull();
                learningDelivery.LearningDeliveryValues.LearnDelHistDaysThisApp.Should().Be((DateTime.Now - expectedPriceEpisodeStartDate).Days);
                //learningDelivery.LearningDeliveryValues.LearnDelHistProgEarnings.Should().Be(earningEpisode.Instalments
                //    .Where(i => i.AcademicYear == short.Parse(_collectionCalendarResponse.AcademicYear))
                //    .Sum(i => i.Amount));
            }

            

        }

        //[Test]
        //public async Task AndApprenticeshipStartedInPreviousAcademicYearThenReturnsPriceEpisodesForEachAcademicYear()
        //{
        //    //Arrange
        //    _mockCollectionCalendarApiClient.Reset();
        //    _mockCollectionCalendarApiClient
        //        .Setup(x => x.Get<GetAcademicYearsResponse>(It.IsAny<GetAcademicYearsRequest>()))
        //        .ReturnsAsync(BuildCollectionCalendarResponse(_apprenticeshipsResponse, false));

        //    // Act
        //    _result = await _handler.Handle(_query, CancellationToken.None);

        //    // Assert
        //    _result.Should().NotBeNull();

        //    var apprenticeship = _apprenticeshipsResponse.Apprenticeships
        //        .MinBy(x => x.Episodes
        //            .Min(episode => episode.Prices
        //                .Min(price => price.StartDate)));

        //    var apprenticeshipStartDate = apprenticeship.Episodes
        //            .Min(episode => episode.Prices
        //                .Min(price => price.StartDate));

        //    var fm36Learner = _result.FM36Learners
        //        .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

        //    fm36Learner.PriceEpisodes.Count.Should().Be(apprenticeship.Episodes.SelectMany(e => e.Prices).Count() + 1);
        //    fm36Learner.PriceEpisodes.Should().Contain(pe => pe.PriceEpisodeValues.EpisodeStartDate == apprenticeshipStartDate);
        //    fm36Learner.PriceEpisodes.Should().Contain(pe => pe.PriceEpisodeValues.EpisodeStartDate == apprenticeshipStartDate.AddMonths(1));

        //    //foreach (var apprenticeship in _apprenticeshipsResponse.Apprenticeships)
        //    //{
        //    //    var expectedEpisodeStartDate = apprenticeship.Episodes.Min(episode => episode.Prices.Min(price => price.StartDate));

        //    //    var fm36Learner = _result.FM36Learners
        //    //        .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

        //    //    fm36Learner.PriceEpisodes
        //    //        .Select(episode => episode.PriceEpisodeValues.EpisodeStartDate)
        //    //        .Should()
        //    //        .AllBeEquivalentTo(expectedEpisodeStartDate);
        //    //}
        //}

        private GetApprenticeshipsResponse BuildApprenticeshipsResponse(long ukprn)
        {
            var response = new GetApprenticeshipsResponse
            {
                Ukprn = ukprn,
                Apprenticeships = new List<Apprenticeship>
                {
                    //Simple apprenticeship, spans an academic year boundary,
                    // so we can use this to test that a new price episode is created in the fm36 when a new academic year starts
                    new Apprenticeship
                    {
                        Uln = _fixture.Create<int>().ToString(),
                        Key = Guid.NewGuid(),
                        Episodes = new List<Episode>
                        {
                            new Episode
                            {
                                Key = Guid.NewGuid(),
                                TrainingCode = _fixture.Create<int>().ToString(),
                                Prices = new List<EpisodePrice>
                                {
                                    new EpisodePrice
                                    {
                                        StartDate = new DateTime(2020,1,1),
                                        EndDate = new DateTime(2021,1,1),
                                        TrainingPrice = 14000,
                                        EndPointAssessmentPrice = 1000,
                                        TotalPrice = 15000,
                                        FundingBandMaximum = 19000
                                    }
                                }
                            }
                        },
                        StartDate = new DateTime(2020,1,1),
                        PlannedEndDate = new DateTime(2021,1,1),
                        AgeAtStartOfApprenticeship = 18
                    },
                    //Apprenticeship with a price change
                    new Apprenticeship
                    {
                        Uln = _fixture.Create<int>().ToString(),
                        Key = Guid.NewGuid(),
                        Episodes = new List<Episode>
                        {
                            new Episode
                            {
                                Key = Guid.NewGuid(),
                                TrainingCode = _fixture.Create<int>().ToString(),
                                Prices = new List<EpisodePrice>
                                {
                                    new EpisodePrice
                                    {
                                        StartDate = new DateTime(2020,8,1),
                                        EndDate = new DateTime(2021,5,2),
                                        TrainingPrice = 21000,
                                        EndPointAssessmentPrice = 1500,
                                        TotalPrice = 22500,
                                        FundingBandMaximum = 30000
                                    },
                                    new EpisodePrice
                                    {
                                        StartDate = new DateTime(2021,5,3),
                                        EndDate = new DateTime(2021,7,31),
                                        TrainingPrice = 28500,
                                        EndPointAssessmentPrice = 1500,
                                        TotalPrice = 30000,
                                        FundingBandMaximum = 30000
                                    }
                                }
                            }
                        },
                        StartDate = new DateTime(2020,8,1),
                        PlannedEndDate = new DateTime(2021,7,31),
                        AgeAtStartOfApprenticeship = 19
                    }
                }
            };
            response.Ukprn = ukprn;
            response.Apprenticeships.ForEach(x => x.Uln = _fixture.Create<long>().ToString());
            return response;
        }

        private GetFm36DataResponse BuildEarningsResponse(GetApprenticeshipsResponse apprenticeshipsResponse)
        {
            var response = new GetFm36DataResponse
            {
                new SharedOuterApi.InnerApi.Responses.Earnings.Apprenticeship
                {
                    Key = apprenticeshipsResponse.Apprenticeships[0].Key,
                    Ukprn = apprenticeshipsResponse.Ukprn,
                    FundingLineType = _fixture.Create<string>(),
                    Episodes = new List<SharedOuterApi.InnerApi.Responses.Earnings.Episode>
                    {
                        new SharedOuterApi.InnerApi.Responses.Earnings.Episode
                        {
                            Key = Guid.NewGuid(),
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
                            }
                        }
                    }
                },
                new SharedOuterApi.InnerApi.Responses.Earnings.Apprenticeship
                {
                    Key = apprenticeshipsResponse.Apprenticeships[1].Key,
                    Ukprn = apprenticeshipsResponse.Ukprn,
                    FundingLineType = _fixture.Create<string>(),
                    Episodes = new List<SharedOuterApi.InnerApi.Responses.Earnings.Episode>
                    {
                        new SharedOuterApi.InnerApi.Responses.Earnings.Episode
                        {
                            Key = Guid.NewGuid(),
                            NumberOfInstalments = 12,
                            CompletionPayment = 6000,
                            OnProgramTotal = 24000,
                            Instalments = new List<Instalment>
                            {
                                new Instalment{ AcademicYear = 2021, DeliveryPeriod = 1, Amount = 2000 },
                                new Instalment{ AcademicYear = 2021, DeliveryPeriod = 2, Amount = 2000 },
                                new Instalment{ AcademicYear = 2021, DeliveryPeriod = 3, Amount = 2000 },
                                new Instalment{ AcademicYear = 2021, DeliveryPeriod = 4, Amount = 2000 },
                                new Instalment{ AcademicYear = 2021, DeliveryPeriod = 5, Amount = 2000 },
                                new Instalment{ AcademicYear = 2021, DeliveryPeriod = 6, Amount = 2000 },
                                new Instalment{ AcademicYear = 2021, DeliveryPeriod = 7, Amount = 2000 },
                                new Instalment{ AcademicYear = 2021, DeliveryPeriod = 8, Amount = 2000 },
                                new Instalment{ AcademicYear = 2021, DeliveryPeriod = 9, Amount = 2000 },
                                new Instalment{ AcademicYear = 2021, DeliveryPeriod = 10, Amount = 2000 },
                                new Instalment{ AcademicYear = 2021, DeliveryPeriod = 11, Amount = 2000 },
                                new Instalment{ AcademicYear = 2021, DeliveryPeriod = 12, Amount = 2000 }
                            }
                        }
                    }
                }
            };
            response.ForEach(x => x.Ukprn = apprenticeshipsResponse.Ukprn);
            return response;
        }

        private GetAcademicYearsResponse BuildCollectionCalendarResponse(GetApprenticeshipsResponse apprenticeshipsResponse, bool apprenticeshipStartedInCurrentAcademicYear = true)
        {
            return new GetAcademicYearsResponse
            {
                AcademicYear = "2021",
                StartDate = new DateTime(2020, 8, 1),
                EndDate = new DateTime(2021, 7, 31)
            };

            var earliestApprenticeshipStartDate = apprenticeshipsResponse.Apprenticeships
                .Min(x => x.Episodes
                    .Min(episode => episode.Prices
                        .Min(price => price.StartDate)));

            return new GetAcademicYearsResponse
            {
                StartDate = apprenticeshipStartedInCurrentAcademicYear ? earliestApprenticeshipStartDate.AddMonths(-1) : earliestApprenticeshipStartDate.AddMonths(1)
            };
        }

        private void SetupMocks(
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

            _mockCollectionCalendarApiClient
                .Setup(x => x.Get<GetAcademicYearsResponse>(It.IsAny<GetAcademicYearsRequest>()))
                .ReturnsAsync(collectionCalendarResponse);
        }
    }

}