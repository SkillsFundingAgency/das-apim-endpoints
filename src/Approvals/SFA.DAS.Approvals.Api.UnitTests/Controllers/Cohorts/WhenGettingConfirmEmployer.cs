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
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetConfirmEmployer;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Cohorts
{
    [TestFixture]
    public class WhenGettingConfirmEmployer
    {
        private CohortController _controller;
        private Mock<IMediator> _mediator;
        private GetConfirmEmployerQueryResult _queryResult;

        private long _providerId;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _queryResult = fixture.Create<GetConfirmEmployerQueryResult>();

            _providerId = fixture.Create<long>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<GetConfirmEmployerQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new CohortController(Mock.Of<ILogger<DraftApprenticeshipController>>(), _mediator.Object);
        }

        [Test]
        public async Task GetConfirmEmployerDeclaredStandardsIsReturned()
        {
            var result = await _controller.GetConfirmEmployer();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okObjectResult = (OkObjectResult) result;
            Assert.IsInstanceOf<GetConfirmEmployerResponse>(okObjectResult.Value);
            var objectResult = (GetConfirmEmployerResponse) okObjectResult.Value;

            var compare = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });

            var comparisonResult = compare.Compare(_queryResult, objectResult);
            Assert.That(comparisonResult.AreEqual, Is.True);
        }
    }
}
