using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.TrackProgress.Application.Commands;
using SFA.DAS.TrackProgress.Application.DTOs;
using SFA.DAS.TrackProgress.Controllers;

namespace SFA.DAS.TrackProgress.OuterApi.Tests.ControllerTests
{
    public class TrackProgressControllerTests
    {
        private Mock<IMediator> _mediator;
        private TrackProgressController _controller;
        private ProgressDto _progressDto;

        [SetUp]
        public void Arrange()
        {
            _mediator = new Mock<IMediator>();
            _controller = new TrackProgressController(_mediator.Object, Mock.Of<ILogger<TrackProgressController>>());
            _progressDto = Mock.Of<ProgressDto>();
        }

        [Test]
        public async Task WhenCallingAddApprenticeshipProgressThenTrackProgressCommandIsFired()
        {
            await _controller.AddApprenticeshipProgress(1, DateTime.Now, _progressDto);

            _mediator.Verify(x => x.Send(It.Is<TrackProgressCommand>(x => x.Progress == _progressDto), CancellationToken.None), Times.Once);
        }
    }
}
