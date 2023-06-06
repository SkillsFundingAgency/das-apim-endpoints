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
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetHasDeclaredStandards;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Cohorts
{
    [TestFixture]
    public class WhenGettingHasDeclaredStandardsTests
    {
        private CohortController _controller;
        private Mock<IMediator> _mediator;
        private GetHasDeclaredStandardsQueryResult _queryResult;

        private long _providerId;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _queryResult = fixture.Create<GetHasDeclaredStandardsQueryResult>();

            _providerId = fixture.Create<long>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.Is<GetHasDeclaredStandardsQuery>(q =>
                        q.ProviderId == _providerId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new CohortController(Mock.Of<ILogger<DraftApprenticeshipController>>(), _mediator.Object);
        }

        [Test]
        public async Task GetHasDeclaredStandardsResponseIsReturned()
        {
            var result = await _controller.GetHasDeclaredStandards(_providerId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult) result;
            Assert.IsInstanceOf<GetHasDeclaredStandardsResponse>(okObjectResult.Value);
            var objectResult = (GetHasDeclaredStandardsResponse) okObjectResult.Value;

            var compare = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });

            var comparisonResult = compare.Compare(_queryResult, objectResult);
            Assert.IsTrue(comparisonResult.AreEqual);
        }
    }
}
