using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using KellermanSoftware.CompareNetObjects;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetViewDraftApprenticeship;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships
{
    [TestFixture]
    public class GetViewDraftApprenticeshipQueryHandlerTests
    {
        private GetViewDraftApprenticeshipQueryHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiMock;
        private GetDraftApprenticeshipResponse _apiResponse;

        private long _apprenticeshipId;
        private long _cohortId;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _apiResponse = fixture.Create<GetDraftApprenticeshipResponse>();

            _apprenticeshipId = fixture.Create<long>();
            _cohortId = fixture.Create<long>();

            _apiMock = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
            _apiMock.Setup(x => x.GetWithResponseCode<GetDraftApprenticeshipResponse>(It.Is<GetDraftApprenticeshipRequest>(q =>
                        q.DraftApprenticeshipId == _apprenticeshipId && q.CohortId == _cohortId)))
                .ReturnsAsync(new ApiResponse<GetDraftApprenticeshipResponse>(_apiResponse, HttpStatusCode.OK, null));

            _handler = new GetViewDraftApprenticeshipQueryHandler( _apiMock.Object);
        }

        [Test]
        public async Task GetViewDraftApprenticeshipQueryResultIsReturned()
        {
            var result = await _handler.Handle(new GetViewDraftApprenticeshipQuery { CohortId = _cohortId, DraftApprenticeshipId = _apprenticeshipId }, CancellationToken.None);

            Assert.That(result, Is.InstanceOf<GetViewDraftApprenticeshipQueryResult>());

            var compare = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });

            var comparisonResult = compare.Compare(_apiResponse, result);
            Assert.That(comparisonResult.AreEqual, Is.True);
        }

        [Test]
        public async Task NothingIsReturnedWhenNotFound()
        {
            _apiMock.Setup(x => x.GetWithResponseCode<GetDraftApprenticeshipResponse>(It.Is<GetDraftApprenticeshipRequest>(q =>
                    q.DraftApprenticeshipId == _apprenticeshipId && q.CohortId == _cohortId)))
                .ReturnsAsync(new ApiResponse<GetDraftApprenticeshipResponse>(null, HttpStatusCode.NotFound, null));

            var result = await _handler.Handle(new GetViewDraftApprenticeshipQuery { CohortId = _cohortId, DraftApprenticeshipId = _apprenticeshipId }, CancellationToken.None);

            Assert.That(result, Is.Null);
        }
    }
}
