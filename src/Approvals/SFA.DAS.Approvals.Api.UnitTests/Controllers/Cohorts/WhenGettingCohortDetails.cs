using System.Collections.Generic;
using System.Linq;
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
using SFA.DAS.Approvals.Api.Models.Cohorts;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetCohortDetails;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Cohorts
{
    [TestFixture]
    public class WhenGettingCohortDetails
    {
        private CohortController _controller;
        private Mock<IMediator> _mediator;
        private GetCohortDetailsQueryResult _queryResult;

        private long _cohortId;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _queryResult = fixture.Create<GetCohortDetailsQueryResult>();

            _cohortId = fixture.Create<long>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.Is<GetCohortDetailsQuery>(q =>
                        q.CohortId == _cohortId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new CohortController(Mock.Of<ILogger<DraftApprenticeshipController>>(), _mediator.Object);
        }

        [Test]
        public async Task GetCohortDetailsResponseIsReturned()
        {
            var result = await _controller.GetCohortDetails(_cohortId);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okObjectResult = (OkObjectResult)result;
            Assert.That(okObjectResult.Value, Is.InstanceOf<GetCohortDetailsResponse>());
            var objectResult = (GetCohortDetailsResponse)okObjectResult.Value;

            var compare = new CompareLogic(new ComparisonConfig { MembersToIgnore = new List<string> { "DraftApprenticeships", "ApprenticeshipEmailOverlaps" }, IgnoreObjectTypes = true });

            var comparisonResult = compare.Compare(_queryResult, objectResult);
            Assert.That(comparisonResult.AreEqual, Is.True);
        }

        [Test]
        public async Task GetCohortDetailsResponseWithCorrectlyMappedDraftApprenticeshipsIsReturned()
        {
            var result = await _controller.GetCohortDetails(_cohortId);

            var okObjectResult = (OkObjectResult)result;
            var objectResult = (GetCohortDetailsResponse)okObjectResult.Value;

            var compare = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });

            var comparisonResult = compare.Compare(_queryResult.DraftApprenticeships.Last(), objectResult.DraftApprenticeships.Last());
            Assert.That(comparisonResult.AreEqual, Is.True);
        }

        [Test]
        public async Task GetCohortDetailsResponseWithCorrectlyMappedApprenticeshipEmailOverlapsIsReturned()
        {
            var result = await _controller.GetCohortDetails(_cohortId);

            var okObjectResult = (OkObjectResult)result;
            var objectResult = (GetCohortDetailsResponse)okObjectResult.Value;

            var compare = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });

            var comparisonResult = compare.Compare(_queryResult.ApprenticeshipEmailOverlaps.Last(), objectResult.ApprenticeshipEmailOverlaps.Last());
            Assert.That(comparisonResult.AreEqual, Is.True);
        }
    }
}
