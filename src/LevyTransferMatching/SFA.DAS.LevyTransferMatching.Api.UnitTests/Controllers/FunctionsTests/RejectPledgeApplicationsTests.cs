using AutoFixture;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.LevyTransferMatching.Application.Commands.RejectPledgeApplications;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests
{
    public class RejectPledgeApplicationsTests
    {
        private FunctionsController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();

        private RejectPledgeApplicationsRequest _request;

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();
            _request = _fixture.Create<RejectPledgeApplicationsRequest>();

            _controller = new FunctionsController(_mediator.Object, Mock.Of<ILogger<FunctionsController>>());
        }

        [Test]
        public async Task Action_Calls_Handler()
        {
            await _controller.RejectPledgeApplications(_request);

            _mediator.Verify(x =>
                x.Send(It.Is<RejectPledgeApplicationsCommand>(c => 
                c.PledgeId == _request.PledgeId), It.IsAny<CancellationToken>()));
        }
    }
}
