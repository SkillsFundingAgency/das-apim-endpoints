using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using AutoFixture;
using SFA.DAS.LevyTransferMatching.Application.Commands.ApproveAutomaticApplication;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests
{
    [TestFixture]
    public class ApproveAutomaticApplicationTests
    {
        private FunctionsController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();

        private ApproveAutomaticApplicationRequest _request;


        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();
            _request = _fixture.Create<ApproveAutomaticApplicationRequest>();

            _controller = new FunctionsController(_mediator.Object, Mock.Of<ILogger<FunctionsController>>());
        }

        [Test]
        public async Task Action_Calls_Handler()
        {
            var result = await _controller.ApproveAutomaticApplication(_request);

            _mediator.Verify(x =>
                x.Send(It.Is<ApproveAutomaticApplicationCommand>(c => c.ApplicationId == _request.ApplicationId &&
                c.PledgeId == _request.PledgeId), It.IsAny<CancellationToken>()));
        }

    }
}
