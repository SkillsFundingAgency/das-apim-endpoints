﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Pledges;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    public class GetPledgeApplications
    {
        private PledgeController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();
        private GetApplicationsQueryResult _queryResult;
        private int _pledgeId;

        [SetUp]
        public void SetUp()
        {
            _pledgeId = _fixture.Create<int>();

            _mediator = new Mock<IMediator>();
            _queryResult = _fixture.Create<GetApplicationsQueryResult>();
            _mediator.Setup(x => x.Send(It.Is<GetApplicationsQuery>(q => q.PledgeId == _pledgeId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new PledgeController(_mediator.Object, Mock.Of<ILogger<PledgeController>>());
        }

        [Test]
        public async Task GetApplications_Returns_GetApplicationsResponse()
        {
            var controllerResponse = await _controller.PledgeApplications(_pledgeId);

            var okObjectResult = controllerResponse as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            var response = okObjectResult.Value as GetApplicationsResponse;
            Assert.IsNotNull(response);

            Assert.AreEqual(_queryResult.Applications.Count(), response.Applications.Count());
            Assert.AreEqual(_queryResult.Standard.StandardUId, response.Standard.StandardUId);
        }
    }
}
