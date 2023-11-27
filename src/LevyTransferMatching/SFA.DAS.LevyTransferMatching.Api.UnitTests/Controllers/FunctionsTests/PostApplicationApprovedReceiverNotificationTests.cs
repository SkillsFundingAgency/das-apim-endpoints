using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationOutcome;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests
{
    [TestFixture]
    public class PostApplicationApprovedReceiverNotificationTests
    {
        private FunctionsController _controller;
        private Mock<IMediator> _mediator;
        private ApplicationApprovedReceiverNotificationRequest _request;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _request = _fixture.Create<ApplicationApprovedReceiverNotificationRequest>();

            _mediator = new Mock<IMediator>();
            
            _controller = new FunctionsController(_mediator.Object, Mock.Of<Microsoft.Extensions.Logging.ILogger<FunctionsController>>());
        }

        [Test]
        public async Task Post_ApplicationApprovedReceiverNotificationRequest()
        {
            await _controller.ApplicationApprovedReceiverNotification(_request);

            _mediator.Verify(x =>
                x.Send(It.Is<ReceiverApplicationApprovedEmailCommand>(c => c.PledgeId == _request.PledgeId && c.ApplicationId == _request.ApplicationId &&
                                                                           c.EncodedApplicationId == _request.EncodedApplicationId && c.ReceiverId == _request.ReceiverId),
                    It.IsAny<CancellationToken>()));
        }
    }
}
