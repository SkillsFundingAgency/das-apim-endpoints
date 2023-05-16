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
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.Apprentices.Queries.CheckReviewApprenticeshipCourse;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices
{
    [TestFixture]
    public class WhenGettingReviewApprenticeshipCourseTests
    {
        private ApprenticesController _controller;
        private Mock<IMediator> _mediator;
        private CheckReviewApprenticeshipCourseQueryResult _queryResult;

        private long _apprenticeshipId;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _queryResult = fixture.Create<CheckReviewApprenticeshipCourseQueryResult>();

            _apprenticeshipId = fixture.Create<long>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.Is<CheckReviewApprenticeshipCourseQuery>(q =>
                        q.ApprenticeshipId == _apprenticeshipId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new ApprenticesController(Mock.Of<ILogger<ApprenticesController>>(), _mediator.Object);
        }

        [Test]
        public async Task CheckReviewApprenticeshipCourseResponseIsReturned()
        {
            var result = await _controller.CheckReviewApprenticeshipCourse(_apprenticeshipId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult) result;
            Assert.IsInstanceOf<CheckReviewApprenticeshipCourseResponse>(okObjectResult.Value);
            var objectResult = (CheckReviewApprenticeshipCourseResponse) okObjectResult.Value;

            var compare = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });

            var comparisonResult = compare.Compare(_queryResult, objectResult);
            Assert.IsTrue(comparisonResult.AreEqual);
        }
    }
}
