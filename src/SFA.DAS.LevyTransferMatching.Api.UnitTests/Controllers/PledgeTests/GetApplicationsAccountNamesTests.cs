﻿using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Pledges;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationsAccountNames;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    public class GetApplicationsAccountNamesTests
    {
        private PledgeController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();
        private GetApplicationsAccountNamesQueryResult _queryResult;
        private int _pledgeId;

        [SetUp]
        public void SetUp()
        {
            _pledgeId = _fixture.Create<int>();

            _mediator = new Mock<IMediator>();
            _queryResult = _fixture.Create<GetApplicationsAccountNamesQueryResult>();
            _mediator.Setup(x => x.Send(It.Is<GetApplicationsAccountNamesQuery>(q => q.PledgeId == _pledgeId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new PledgeController(_mediator.Object, Mock.Of<ILogger<PledgeController>>());
        }

        [Test]
        public async Task GetApplicationsAccountNames_Returns_GetApplicationsAccountNamesResponse()
        {
            var controllerResponse = await _controller.ApplicationsAccountNames(_pledgeId);

            var okObjectResult = controllerResponse as OkObjectResult;
            var response = okObjectResult.Value as GetApplicationsAccountNamesResponse;

            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(response);
            Assert.AreEqual(_queryResult.Applications.Count(), response.Applications.Count());
        }
    }
}
