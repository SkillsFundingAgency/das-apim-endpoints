using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Api.Controllers;
using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprenticeFeedback;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.UnitTests.Controllers
{
    public class WhenPostingCreateApprenticeFeedback
    {
        private Mock<IMediator> _mockMediator;
        private ApprenticeFeedbackController _controller;

        [SetUp]
        public void Arrange()
        {
            _mockMediator = new Mock<IMediator>();

            _controller = new ApprenticeFeedbackController(_mockMediator.Object, Mock.Of<ILogger<ApprenticeFeedbackController>>());
        }

        [Test, MoqAutoData]
        public async Task And_CommandIsProcessedSuccessfully_Then_ReturnCreated(CreateApprenticeFeedbackCommand request)
        {
            var result = await _controller.CreateFeedbackTarget(request);

            result.Should().NotBeNull();
        }
    }
}
