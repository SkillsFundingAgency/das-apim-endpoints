using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipPriorLearningSummary;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships
{
    [TestFixture]
    public class GetEditDraftApprenticeshipPriorLearningSummaryQueryHandlerTests
    {
        private GetEditDraftApprenticeshipPriorLearningSummaryQueryHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
        private GetDraftApprenticeshipResponse _draftApprenticeship;
        private GetPriorLearningSummaryResponse _rplSummary;
        private GetEditDraftApprenticeshipPriorLearningSummaryQuery _query;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _draftApprenticeship = fixture.Create<GetDraftApprenticeshipResponse>();
            _rplSummary = fixture.Create<GetPriorLearningSummaryResponse>();

            _query = fixture.Create<GetEditDraftApprenticeshipPriorLearningSummaryQuery>();

            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _apiClient.Setup(x =>
                    x.Get<GetDraftApprenticeshipResponse>(It.Is<GetDraftApprenticeshipRequest>(r => r.CohortId == _query.CohortId && r.DraftApprenticeshipId == _query.DraftApprenticeshipId)))
                .ReturnsAsync(_draftApprenticeship);

            _apiClient.Setup(x =>
                    x.Get<GetPriorLearningSummaryResponse>(It.Is<GetPriorLearningSummaryRequest>(r => r.CohortId == _query.CohortId && r.DraftApprenticeshipId == _query.DraftApprenticeshipId)))
                .ReturnsAsync(_rplSummary);

            _handler = new GetEditDraftApprenticeshipPriorLearningSummaryQueryHandler(_apiClient.Object);
        }

        [Test]
        public async Task Handle_TrainingTotalHours_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_rplSummary.TrainingTotalHours, Is.EqualTo(result.TrainingTotalHours));
        }

        [Test]
        public async Task Handle_DurationReducedByHours_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_rplSummary.DurationReducedByHours, Is.EqualTo(result.DurationReducedByHours));
        }

        [Test]
        public async Task Handle_PriceReduced_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_rplSummary.PriceReducedBy, Is.EqualTo(result.PriceReducedBy));
        }

        [Test]
        public async Task Handle_FundingBandMaximum_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_rplSummary.FundingBandMaximum, Is.EqualTo(result.FundingBandMaximum));
        }

        [Test]
        public async Task Handle_PercentageOfPriorLearning_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_rplSummary.PercentageOfPriorLearning, Is.EqualTo(result.PercentageOfPriorLearning));
        }

        [Test]
        public async Task Handle_MinimumPercentageReduction_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_rplSummary.MinimumPercentageReduction, Is.EqualTo(result.MinimumPercentageReduction));
        }

        [Test]
        public async Task Handle_MinimumPriceReduction_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_rplSummary.MinimumPriceReduction, Is.EqualTo(result.MinimumPriceReduction));
        }

        [Test]
        public async Task Handle_RplPriceReductionError_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_rplSummary.RplPriceReductionError, Is.EqualTo(result.RplPriceReductionError));
        }

        [Test]
        public async Task Handle_TotalCost_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.Cost, Is.EqualTo(result.TotalCost));
        }

        [Test]
        public async Task Handle_LastName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.LastName, Is.EqualTo(result.LastName));
        }

        [Test]
        public async Task Handle_FirstName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.FirstName, Is.EqualTo(result.FirstName));
        }

        [Test]
        public async Task Handle_HasStandardOptions_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.HasStandardOptions, Is.EqualTo(result.HasStandardOptions));
        }
    }
}
