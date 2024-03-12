using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using KellermanSoftware.CompareNetObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.Apprentices.Queries.GetReviewApprenticeshipUpdates;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices
{
    [TestFixture]
    public class WhenGettingReviewApprenticeshipUpdates
    {
        private ApprenticesController _controller;
        private Mock<IMediator> _mediator;
        private GetReviewApprenticeshipUpdatesQueryResult _queryResult;

        private long _apprenticeshipId;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _queryResult = fixture.Create<GetReviewApprenticeshipUpdatesQueryResult>();

            _apprenticeshipId = fixture.Create<long>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.Is<GetReviewApprenticeshipUpdatesQuery>(q =>
                        q.ApprenticeshipId == _apprenticeshipId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new ApprenticesController(Mock.Of<ILogger<ApprenticesController>>(), _mediator.Object, Mock.Of<IMapper>());
        }

        [Test]
        public async Task GetReviewApprenticeshipUpdatesResponseIsReturned()
        {
            var result = await _controller.GetReviewApprenticeshipUpdates(_apprenticeshipId);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okObjectResult = (OkObjectResult) result;
            Assert.That(okObjectResult.Value, Is.InstanceOf<GetReviewApprenticeshipUpdatesResponse>());
            var objectResult = (GetReviewApprenticeshipUpdatesResponse) okObjectResult.Value;

            var compare = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });

            var comparisonResult = compare.Compare(_queryResult, objectResult);
            Assert.That(comparisonResult.AreEqual, Is.True);
        }
    }
}
