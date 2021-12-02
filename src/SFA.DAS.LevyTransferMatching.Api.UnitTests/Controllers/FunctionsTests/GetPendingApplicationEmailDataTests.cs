using AutoFixture;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests
{
    [TestFixture]
    public class GetPendingApplicationEmailDataTests
    {
        private FunctionsController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();

            _controller = new FunctionsController(_mediator.Object, Mock.Of<ILogger<FunctionsController>>());
        }

        [Test]
        public async Task Get_PendingApplicationEmailData_Returns_Response()
        {
            var result = await _controller.GetPendingApplicationEmailData();

            _mediator.Verify(x =>
                x.Send(It.IsAny<GetPendingApplicationEmailDataQuery>(),
                    It.IsAny<CancellationToken>()));
        }
    }
}
