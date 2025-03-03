using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Api.Controllers;
using SFA.DAS.ApprenticeFeedback.Application.Commands.ProcessFeedbackTargetVariants;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.UnitTests.Controllers
{
    public class WhenPostingFeedbackTargetVariants
    {
        private Mock<IMediator> _mockMediator;
        private FeedbackTargetVariantController _controller;

        [SetUp]
        public void Arrange()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new FeedbackTargetVariantController(_mockMediator.Object, Mock.Of<ILogger<FeedbackTargetVariantController>>());
        }

        [Test, MoqAutoData]
        public async Task And_CommandIsProcessedSuccessfully_Then_OkReturned(ProcessFeedbackTargetVariantsCommand command)
        {
            var result = await _controller.ProcessFeedbackTargetVariants(command);
            result.Result.Should().BeOfType<OkResult>();
        }
    }
}
