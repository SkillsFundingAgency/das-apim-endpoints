﻿using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunities;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.OpportunityTests
{
    [TestFixture]
    public class ApplyTests
    {
        private OpportunityController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();

        private int _opportunityId;
        private long _accountId;
        private ApplyRequest _request;
        private CreateApplicationCommandResult _result;

        [SetUp]
        public void SetUp()
        {
            _opportunityId = _fixture.Create<int>();
            _accountId = _fixture.Create<long>();
            _request = _fixture.Create<ApplyRequest>();
            _result = _fixture.Create<CreateApplicationCommandResult>();

            _mediator = new Mock<IMediator>();

            _controller = new OpportunityController(_mediator.Object);

            _mediator.Setup(x => x.Send(It.Is<CreateApplicationCommand>(command =>
                    command.PledgeId == _opportunityId &&
                    command.EmployerAccountId == _accountId
                ), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_result);
        }

        [Test]
        public async Task Post_Returns_ApplicationId()
        {
            var actionResult = await _controller.Apply(_accountId, _opportunityId, _request);
            var createdResult = actionResult as CreatedResult;
            Assert.IsNotNull(createdResult);
            var response = createdResult.Value as ApplyResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual(_result.ApplicationId, response.ApplicationId);
        }
    }
}
