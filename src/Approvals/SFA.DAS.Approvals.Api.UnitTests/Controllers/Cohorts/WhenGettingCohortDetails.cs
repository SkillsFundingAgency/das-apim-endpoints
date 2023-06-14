﻿using System.Threading;
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
        private long _providerId;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _queryResult = fixture.Create<GetCohortDetailsQueryResult>();

            _cohortId = fixture.Create<long>();
            _providerId = fixture.Create<long>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.Is<GetCohortDetailsQuery>(q =>
                        q.CohortId == _cohortId && q.ProviderId == _providerId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new CohortController(Mock.Of<ILogger<DraftApprenticeshipController>>(), _mediator.Object);
        }

        [Test]
        public async Task GetCohortDetailsResponseIsReturned()
        {
            var result = await _controller.GetCohortDetails(_cohortId, _providerId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            Assert.IsInstanceOf<GetCohortDetailsResponse>(okObjectResult.Value);
            var objectResult = (GetCohortDetailsResponse)okObjectResult.Value;

            var compare = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });

            var comparisonResult = compare.Compare(objectResult, objectResult);
            Assert.IsTrue(comparisonResult.AreEqual);
        }
    }
}
