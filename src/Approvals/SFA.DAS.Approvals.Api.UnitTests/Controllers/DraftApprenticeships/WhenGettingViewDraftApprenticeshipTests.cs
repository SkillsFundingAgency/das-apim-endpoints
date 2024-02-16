using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using KellermanSoftware.CompareNetObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models.DraftApprenticeships;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetViewDraftApprenticeship;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.DraftApprenticeships
{
    [TestFixture]
    public class WhenGettingViewDraftApprenticeshipTests
    {
        private DraftApprenticeshipController _controller;
        private Mock<IMediator> _mediator;
        private GetViewDraftApprenticeshipQueryResult _queryResult;

        private long _cohortId;
        private long _draftApprenticeshipId;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _queryResult = fixture.Create<GetViewDraftApprenticeshipQueryResult>();

            _cohortId = fixture.Create<long>();
            _draftApprenticeshipId = fixture.Create<long>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.Is<GetViewDraftApprenticeshipQuery>(q =>
                        q.CohortId == _cohortId &&
                        q.DraftApprenticeshipId == _draftApprenticeshipId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new DraftApprenticeshipController(Mock.Of<ILogger<DraftApprenticeshipController>>(), _mediator.Object);
        }

        [Test]
        public async Task ViewDraftApprenticeshipResponseIsReturned()
        {
            var result = await _controller.GetViewDraftApprenticeship(_cohortId, _draftApprenticeshipId);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okObjectResult = (OkObjectResult) result;
            Assert.That(okObjectResult.Value, Is.InstanceOf<GetViewDraftApprenticeshipResponse>());
            var objectResult = (GetViewDraftApprenticeshipResponse)okObjectResult.Value;

            var compare = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });

            var comparisonResult = compare.Compare(_queryResult, objectResult);
            Assert.That(comparisonResult.AreEqual, Is.True);
        }

        [Test]
        public async Task NotFoundResponseIsReturned()
        {
            var result = await _controller.GetViewDraftApprenticeship(_cohortId+1, _draftApprenticeshipId+1);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
