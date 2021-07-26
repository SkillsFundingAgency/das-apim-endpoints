﻿using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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

            _controller = new OpportunityController(_mediator.Object);

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
            Assert.IsNotNull(createdResult);
            var response = createdResult.Value as GetConfirmationResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual(_result.AccountName, response.AccountName);
            Assert.AreEqual(_result.IsNamePublic, response.IsNamePublic);
        }
    }
}
