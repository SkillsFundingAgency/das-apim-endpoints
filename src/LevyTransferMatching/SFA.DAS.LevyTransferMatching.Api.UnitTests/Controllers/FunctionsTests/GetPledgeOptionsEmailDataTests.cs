using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests
{
    [TestFixture]
    public class GetPledgeOptionsEmailDataTests
    {
        private FunctionsController _controller;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();

            _controller = new FunctionsController(_mediator.Object, Mock.Of<ILogger<FunctionsController>>());
        }

        [Test]
        public async Task Get_PledgeOptionsEmailData_Returns_Response()
        {
            var result = await _controller.GetPledgeOptionsEmailData();

            _mediator.Verify(x =>
                x.Send(It.IsAny<GetPledgeOptionsEmailDataQuery>(),
                    It.IsAny<CancellationToken>()));
        }
    }
}
