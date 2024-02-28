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

            _mediator.Setup(x => x.Send(It.Is<GetConfirmationQuery>(query =>
                    query.OpportunityId == _opportunityId
                ), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_result);
        }

        [Test]
        public async Task Get_Returns_GetConfirmationResponse()
        {
            var actionResult = await _controller.Confirmation(_accountId, _opportunityId);
            var createdResult = actionResult as OkObjectResult;
            Assert.That(createdResult, Is.Not.Null);
            var response = createdResult.Value as GetConfirmationResponse;
            Assert.That(response, Is.Not.Null);
            Assert.That(_result.AccountName, Is.EqualTo(response.AccountName));
            Assert.That(_result.IsNamePublic, Is.EqualTo(response.IsNamePublic));
        }
    }
}
