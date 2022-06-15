using AutoFixture;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests
{
    [TestFixture]
    public class DebitApplicationTests
    {
        private FunctionsController _controller;
        private Mock<IMediator> _mediator;
        private DebitApplicationRequest _request;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _request = _fixture.Create<DebitApplicationRequest>();

            _mediator = new Mock<IMediator>();

            _controller = new FunctionsController(_mediator.Object, Mock.Of<Microsoft.Extensions.Logging.ILogger<FunctionsController>>());
        }

        [Test]
        public async Task Post_DebitApplicationRequest_Debits_Application()
        {
            await _controller.DebitApplication(_request);

            _mediator.Verify(x =>
                x.Send(It.Is<DebitApplicationCommand>(c => c.ApplicationId == _request.ApplicationId && c.Amount == _request.Amount && c.NumberOfApprentices == _request.NumberOfApprentices),
                    It.IsAny<CancellationToken>()));
        }
    }
}
