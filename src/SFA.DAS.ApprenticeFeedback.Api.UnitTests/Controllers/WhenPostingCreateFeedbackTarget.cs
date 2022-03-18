using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Api.Controllers;
using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateFeedbackTarget;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.UnitTests.Controllers
{
    public class WhenPostingCreateFeedbackTarget
    {
        private Mock<IMediator> _mockMediator;

        private FeedbackTargetController _controller;

        [SetUp]
        public void Arrange()
        {
            _mockMediator = new Mock<IMediator>();

            _controller = new FeedbackTargetController(_mockMediator.Object, Mock.Of<ILogger<FeedbackTargetController>>());
        }

        [Test, MoqAutoData]
        public async Task And_CommandIsProcessedSuccessfully_Then_ReturnCreated(CreateFeedbackTargetCommand request)
        {
            var result = await _controller.CreateFeedbackTarget(request);

            result.Should().NotBeNull();
        }
    }
}
