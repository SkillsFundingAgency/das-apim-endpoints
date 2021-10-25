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

            _controller = new PledgeController(_mediator.Object, Mock.Of<ILogger<PledgeController>>());
        }

        [Test]
        public async Task Amount_Returns_GetCreateResponse()
        {
            _mediator.SetupMediatorResponseToReturnAsync<GetAmountQueryResult, GetAmountQuery>(_queryResult, o => o.EncodedAccountId == _encodedAccountId);

            var controllerResponse = await _controller.Amount(_encodedAccountId);

            var okObjectResult = controllerResponse as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            var response = okObjectResult.Value as GetAmountResponse;
            Assert.IsNotNull(response);

            Assert.AreEqual(_queryResult.RemainingTransferAllowance, response.RemainingTransferAllowance);
            Assert.AreEqual(_queryResult.DasAccountName, response.DasAccountName);
        }

        [Test]
        public async Task GetCreate_Returns_GetCreateResponse()
        {
            _mediator.SetupMediatorResponseToReturnAsync<GetAmountQueryResult, GetAmountQuery>(null, o => o.EncodedAccountId == _encodedAccountId);

            var controllerResponse = await _controller.Amount(_encodedAccountId);

            var result = controllerResponse as NotFoundResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }
    }
}