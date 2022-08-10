
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Api.Controllers;
using SFA.DAS.ApprenticeFeedback.Application.Commands.GenerateEmailTransaction;
using SFA.DAS.Testing.AutoFixture;


namespace SFA.DAS.ApprenticeFeedback.Api.UnitTests.Controllers
{
    public class WhenPostingFeedbackTransaction
    {
        private Mock<IMediator> _mockMediator;

        private FeedbackTransactionController _controller;

        [SetUp]
        public void Arrange()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new FeedbackTransactionController(_mockMediator.Object, 
                                                            Mock.Of<ILogger<FeedbackTransactionController>>());
        }

        [Test, MoqAutoData]
        public async Task And_CommandIsProcessedSuccessfully_Then_ReturnResults()
        {
            var result = await _controller.GenerateEmailTransaction();
            result.Should().NotBeNull();
        }
    }
}
