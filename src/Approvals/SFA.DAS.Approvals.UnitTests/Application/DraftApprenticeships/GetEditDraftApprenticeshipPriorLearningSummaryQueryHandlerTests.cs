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
            Assert.AreEqual(_rplSummary.TrainingTotalHours, result.TrainingTotalHours);
        }

        [Test]
        public async Task Handle_DurationReducedByHours_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_rplSummary.DurationReducedByHours, result.DurationReducedByHours);
        }

        [Test]
        public async Task Handle_CostBeforeRpl_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_rplSummary.CostBeforeRpl, result.CostBeforeRpl);
        }

        [Test]
        public async Task Handle_PriceReduced_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_rplSummary.PriceReducedBy, result.PriceReducedBy);
        }

        [Test]
        public async Task Handle_FundingBandMaximum_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_rplSummary.FundingBandMaximum, result.FundingBandMaximum);
        }

        [Test]
        public async Task Handle_PercentageOfPriorLearning_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_rplSummary.PercentageOfPriorLearning, result.PercentageOfPriorLearning);
        }

        [Test]
        public async Task Handle_MinimumPercentageReduction_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_rplSummary.MinimumPercentageReduction, result.MinimumPercentageReduction);
        }

        [Test]
        public async Task Handle_MinimumPriceReduction_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_rplSummary.MinimumPriceReduction, result.MinimumPriceReduction);
        }

        [Test]
        public async Task Handle_RplPriceReductionError_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_rplSummary.RplPriceReductionError, result.RplPriceReductionError);
        }

        [Test]
        public async Task Handle_TotalCost_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.Cost, result.TotalCost);
        }

        [Test]
        public async Task Handle_LastName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.LastName, result.LastName);
        }

        [Test]
        public async Task Handle_FirstName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.FirstName, result.FirstName);
        }

        [Test]
        public async Task Handle_HasStandardOptions_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.HasStandardOptions, result.HasStandardOptions);
        }
    }
}
