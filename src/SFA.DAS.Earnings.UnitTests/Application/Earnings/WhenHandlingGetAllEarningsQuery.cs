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
        public async Task AndAPriceEpisodeIsCreatedForEachPrice()
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
            var response = _fixture.Create<GetApprenticeshipsResponse>();
            response.Ukprn = ukprn;
            response.Apprenticeships.ForEach(x => x.Uln = _fixture.Create<long>().ToString());
            return response;
        }

        private GetFm36DataResponse BuildEarningsResponse(GetApprenticeshipsResponse apprenticeshipsResponse)
        {
            var response = _fixture.Create<GetFm36DataResponse>();
            response.ForEach(x => x.Ukprn = apprenticeshipsResponse.Ukprn);
            return response;
        }

        private GetAcademicYearsResponse BuildCollectionCalendarResponse(GetApprenticeshipsResponse apprenticeshipsResponse, bool apprenticeshipStartedInCurrentAcademicYear = true)
        {
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