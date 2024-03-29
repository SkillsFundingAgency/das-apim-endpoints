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
    public class GetPledgeApplicationsTests
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
            var controllerResponse = await _controller.PledgeApplications(_pledgeId, string.Empty, string.Empty);

            var okObjectResult = controllerResponse as OkObjectResult;
            var response = okObjectResult.Value as GetApplicationsResponse;

            Assert.That(okObjectResult, Is.Not.Null);
            Assert.That(response, Is.Not.Null);
            Assert.That(response.AutomaticApprovalOption, Is.Not.Null);
            Assert.That(response.Applications.Count(), Is.EqualTo(_queryResult.Applications.Count()));
            Assert.That(response.PledgeRemainingAmount, Is.EqualTo(_queryResult.RemainingAmount));
            Assert.That(response.PledgeTotalAmount, Is.EqualTo(_queryResult.TotalAmount));
        }
    }
}
