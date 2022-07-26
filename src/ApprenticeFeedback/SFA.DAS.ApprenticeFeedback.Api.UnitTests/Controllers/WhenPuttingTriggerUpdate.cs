using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Api.Controllers;
using SFA.DAS.ApprenticeFeedback.Application.Commands.TriggerFeedbackTargetUpdate;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.UnitTests.Controllers
{
    public class WhenPuttingTriggerUpdate
    {
        private Mock<IMediator> _mockMediator;
        private ApprenticeFeedbackTargetController _controller;

        [SetUp]
        public void Arrange()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new ApprenticeFeedbackTargetController(_mockMediator.Object, Mock.Of<ILogger<ApprenticeFeedbackTargetController>>());
        }

        [Test, MoqAutoData]
        public async Task And_CommandIsProcessedSuccessfully_Then_ReturnOk(TriggerFeedbackTargetUpdateCommand request)
        {
            var result = await _controller.TriggerUpdate(request);
            result.Should().NotBeNull();
        }
    }
}
