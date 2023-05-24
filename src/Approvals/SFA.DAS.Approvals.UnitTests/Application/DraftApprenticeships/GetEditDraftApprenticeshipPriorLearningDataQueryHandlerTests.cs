using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipPriorLearningData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships
{
    [TestFixture]
    public class GetEditDraftApprenticeshipPriorLearningDataQueryHandlerTests
    {
        private GetEditDraftApprenticeshipPriorLearningDataQueryHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
        private GetDraftApprenticeshipResponse _draftApprenticeship;
        private GetEditDraftApprenticeshipPriorLearningDataQuery _query;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _draftApprenticeship = fixture.Build<GetDraftApprenticeshipResponse>()
                .With(x => x.IsDurationReducedByRpl, true).With(x => x.DurationReducedBy, 100).Create();
            _query = fixture.Create<GetEditDraftApprenticeshipPriorLearningDataQuery>();

            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetDraftApprenticeshipResponse>(It.Is<GetDraftApprenticeshipRequest>(r => r.CohortId == _query.CohortId && r.DraftApprenticeshipId == _query.DraftApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetDraftApprenticeshipResponse>(_draftApprenticeship, HttpStatusCode.OK, string.Empty));

            _handler = new GetEditDraftApprenticeshipPriorLearningDataQueryHandler(_apiClient.Object);
        }

        [Test]
        public async Task Handle_TrainingTotalHours_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.TrainingTotalHours, result.TrainingTotalHours);
        }

        [Test]
        public async Task Handle_DurationReducedByHours_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.DurationReducedByHours, result.DurationReducedByHours);
        }

        [TestCase(true, 100, true)]
        [TestCase(true, null, true)]
        [TestCase(false, null, false)]
        [TestCase(false, 100, false)]
        [TestCase(null, null, null)]
        [TestCase(null, 100, true)]
        public async Task Handle_IsDurationReducedByRpl_Is_Mapped(bool? isDurationReducedByRpl, int? durationReducedBy, bool? expected)
        {
            _draftApprenticeship.IsDurationReducedByRpl = isDurationReducedByRpl;
            _draftApprenticeship.DurationReducedBy = durationReducedBy;
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(expected, result.IsDurationReducedByRpl);
        }

        [TestCase(true, 100, 100)]
        [TestCase(true, null, null)]
        [TestCase(false, null, null)]
        [TestCase(false, 100, null)]
        [TestCase(null, null, null)]
        [TestCase(null, 100, 100)]
        public async Task Handle_DurationReducedBy_Is_Mapped(bool? isDurationReducedByRpl, int? durationReducedBy, int? expected)
        {
            _draftApprenticeship.IsDurationReducedByRpl = isDurationReducedByRpl;
            _draftApprenticeship.DurationReducedBy = durationReducedBy;
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(expected, result.DurationReducedBy);
        }

        [Test]
        public async Task Handle_PriceReduced_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.PriceReducedBy, result.PriceReduced);
        }
    }
}
