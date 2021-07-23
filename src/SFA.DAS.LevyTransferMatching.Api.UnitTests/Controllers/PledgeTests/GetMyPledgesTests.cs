using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Pledges;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetMyPledges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    [TestFixture]
    public class GetMyPledgesTests
    {
        private PledgeController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();
        private GetMyPledgesQueryResult _queryResult;
        private int _accountId;

        [SetUp]
        public void SetUp()
        {
            _mediator = new Mock<IMediator>();
            _queryResult = _fixture.Create<GetMyPledgesQueryResult>();
            _accountId = _fixture.Create<int>();
            _mediator.Setup(x => x.Send(It.IsAny<GetMyPledgesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new PledgeController(_mediator.Object, Mock.Of<ILogger<PledgeController>>());
        }

        [Test]
        public async Task GetMyPledges_Returns_GetMyPledgesResponse()
        {
            var controllerResponse = await _controller.MyPledges(_accountId);

            var okObjectResult = controllerResponse as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            var response = okObjectResult.Value as GetMyPledgesResponse;
            Assert.IsNotNull(response);

            Assert.IsNotNull(response.Pledges);
            CollectionAssert.IsNotEmpty(response.Pledges);
            Assert.That(!response.Pledges.Any(x => x.Id == 0));
            Assert.That(!response.Pledges.Any(x => x.Amount == 0));
        }
    }
}
