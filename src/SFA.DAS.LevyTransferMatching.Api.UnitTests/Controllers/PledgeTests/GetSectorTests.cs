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
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetSector;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    [TestFixture]
    public class GetSectorTests
    {
        private PledgeController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();
        private GetSectorQueryResult _queryResult;

        [SetUp]
        public void SetUp()
        {
            _mediator = new Mock<IMediator>();
            _queryResult = _fixture.Create<GetSectorQueryResult>();
            _mediator.Setup(x => x.Send(It.IsAny<GetSectorQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new PledgeController(_mediator.Object, Mock.Of<ILogger<PledgeController>>());
        }

        [Test]
        public async Task GetSector_Returns_GetSectorResponse()
        {
            var controllerResponse = await _controller.Sector();

            var okObjectResult = controllerResponse as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            var response = okObjectResult.Value as GetSectorResponse;
            Assert.IsNotNull(response);

            Assert.AreEqual(_queryResult.Sectors, response.Sectors);
        }
    }
}