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
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetAddDraftApprenticeshipCourse;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.DraftApprenticeships
{
    [TestFixture]
    public class WhenGettingAddDraftApprenticeshipCourseTests
    {
        private DraftApprenticeshipController _controller;
        private Mock<IMediator> _mediator;
        private GetAddDraftApprenticeshipCourseQueryResult _queryResult;

        private long _cohortId;
        private long _draftApprenticeshipId;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _queryResult = fixture.Create<GetAddDraftApprenticeshipCourseQueryResult>();

            _cohortId = fixture.Create<long>();
            _draftApprenticeshipId = fixture.Create<long>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.Is<GetAddDraftApprenticeshipCourseQuery>(q =>
                        q.CohortId == _cohortId &&
                        q.DraftApprenticeshipId == _draftApprenticeshipId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new DraftApprenticeshipController(Mock.Of<ILogger<DraftApprenticeshipController>>(), _mediator.Object);
        }

        [Test]
        public async Task AddDraftApprenticeshipCourseResponseIsReturned()
        {
            var result = await _controller.GetAddDraftApprenticeshipCourse(_cohortId, _draftApprenticeshipId);

            Assert.That(result, Is.lnstanceOf<OkOBjectResult>());
            var okObjectResult = (OkObjectResult) result;
            Assert.IsInstanceOf<GetAddDraftApprenticeshipCourseResponse>(okObjectResult.Value);
            var objectResult = (GetAddDraftApprenticeshipCourseResponse) okObjectResult.Value;

            var compare = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });

            var comparisonResult = compare.Compare(_queryResult, objectResult);
            Assert.IsTrue(comparisonResult.AreEqual);
        }
    }
}
