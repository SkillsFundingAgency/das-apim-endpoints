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
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetAmount;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    [TestFixture]
    public class GetAmountTests
    {
        private PledgeController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();
        private GetAmountQueryResult _queryResult;
        private string _encodedAccountId;

        [SetUp]
        public void SetUp()
        {
            _encodedAccountId = _fixture.Create<string>();

            _mediator = new Mock<IMediator>();
            _queryResult = _fixture.Create<GetAmountQueryResult>();
            _mediator.Setup(x => x.Send(It.Is<GetAmountQuery>(q => q.EncodedAccountId == _encodedAccountId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new PledgeController(_mediator.Object, Mock.Of<ILogger<PledgeController>>());
        }

        [Test]
        public async Task GetCreate_Returns_GetCreateResponse()
        {
            var controllerResponse = await _controller.Amount(_encodedAccountId);

            var okObjectResult = controllerResponse as OkObjectResult;
            Assert.That(okObjectResult, Is.Not.Null);
            var response = okObjectResult.Value as GetAmountResponse;
            Assert.That(response, Is.Not.Null);

            Assert.That(_queryResult.RemainingTransferAllowance, Is.EqualTo(response.RemainingTransferAllowance));
            Assert.That(_queryResult.StartingTransferAllowance, Is.EqualTo(response.StartingTransferAllowance));

        }
    }
}