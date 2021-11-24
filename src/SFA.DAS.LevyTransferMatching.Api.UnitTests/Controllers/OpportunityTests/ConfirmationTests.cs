using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunities;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunities.GetConfirmation;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.OpportunityTests
{
    [TestFixture]
    public class ConfirmationTests
    {
        private OpportunityController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();

        private int _opportunityId;
        private long _accountId;
        private GetConfirmationQueryResult _result;

        [SetUp]
        public void SetUp()
        {
            _opportunityId = _fixture.Create<int>();
            _accountId = _fixture.Create<long>();

            _result = _fixture.Create<GetConfirmationQueryResult>();

            _mediator = new Mock<IMediator>(MockBehavior.Strict);

            _controller = new OpportunityController(Mock.Of<ILogger<OpportunityController>>(), _mediator.Object);
        }

        [Test]
        public async Task Get_Returns_GetConfirmationResponse()
        {
            _mediator.SetupMediatorResponseToReturnAsync<GetConfirmationQueryResult, GetConfirmationQuery>(_result, q => q.OpportunityId == _opportunityId);
        
            var actionResult = await _controller.Confirmation(_accountId, _opportunityId);
            var createdResult = actionResult as OkObjectResult;
            Assert.IsNotNull(createdResult);
            var response = createdResult.Value as GetConfirmationResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual(_result.AccountName, response.AccountName);
            Assert.AreEqual(_result.IsNamePublic, response.IsNamePublic);
        }

        [Test]
        public async Task Get_Given_Incorrect_Parameters_Returns_GetConfirmationResponse()
        {
            _mediator.SetupMediatorResponseToReturnAsync<GetConfirmationQueryResult, GetConfirmationQuery>(null, q => q.OpportunityId == _opportunityId);

            var actionResult = await _controller.Confirmation(_accountId, _opportunityId);
            var result = actionResult as NotFoundResult;
            
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }
    }
}
