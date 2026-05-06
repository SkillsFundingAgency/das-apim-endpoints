using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Api.Models.Cohorts;
using SFA.DAS.Approvals.Application.Cohorts.Commands;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetCohortDetails;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Cohorts
{
    [TestFixture]
    public class WhenPostingCohortDetails
    {
        private CohortController _controller;
        private Mock<IMediator> _mediator;
        private DetailsPostRequest _request;

        private long _cohortId;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _cohortId = fixture.Create<long>();
            _request = fixture.Create<DetailsPostRequest>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.Is<GetCohortDetailsQuery>(q =>
                        q.CohortId == _cohortId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            _controller = new CohortController(Mock.Of<ILogger<DraftApprenticeshipController>>(), _mediator.Object);
        }

        [Test]
        public async Task OkResponseIsReturned()
        {
            var result = await _controller.Details(_cohortId, _request);
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public async Task DetailsAreSubmittedCorrectly()
        {
            await _controller.Details(_cohortId, _request);
            
            _mediator.Verify(x => x.Send(It.Is<PostDetailsCommand>(x =>
                        x.Message == _request.Message
                        && x.SubmissionType == _request.SubmissionType
                        && x.CohortId == _cohortId
                        && x.UserInfo.UserDisplayName == _request.UserInfo.UserDisplayName
                        && x.UserInfo.UserEmail == _request.UserInfo.UserEmail
                        && x.UserInfo.UserId == _request.UserInfo.UserId
                        ), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
