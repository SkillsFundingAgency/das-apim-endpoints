using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Api.Models.Pledges;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetCreate;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    [TestFixture]
    public class GetCreateTests
    {
        private PledgeController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();
        private GetCreateQueryResult _queryResult;

        [SetUp]
        public void SetUp()
        {
            _mediator = new Mock<IMediator>();
            _queryResult = _fixture.Create<GetCreateQueryResult>();
            _mediator.Setup(x => x.Send(It.IsAny<GetCreateQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new PledgeController(_mediator.Object, Mock.Of<ILogger<PledgeController>>());
        }

        [Test]
        public async Task GetCreate_Returns_GetCreateResponse()
        {
            var controllerResponse = await _controller.Create();

            var okObjectResult = controllerResponse as OkObjectResult;
            Assert.That(okObjectResult, Is.Not.Null);
            var response = okObjectResult.Value as GetCreateResponse;
            Assert.That(response, Is.Not.Null);

            Assert.That(_queryResult.Sectors, Is.EqualTo(response.Sectors));
            Assert.That(_queryResult.Levels, Is.EqualTo(response.Levels));
            Assert.That(_queryResult.JobRoles, Is.EqualTo(response.JobRoles));
        }
    }
}
