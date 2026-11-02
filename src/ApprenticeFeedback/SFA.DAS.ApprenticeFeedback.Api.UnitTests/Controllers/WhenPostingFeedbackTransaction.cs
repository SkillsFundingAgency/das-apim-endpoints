using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Api.Controllers;
using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateFeedbackTransaction;
using SFA.DAS.ApprenticeFeedback.Application.Commands.ProcessEmailTransaction;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprentice;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Types.Models;

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

        [Test]
        public async Task And_CreateFeedbackTransactionSucceeds_Then_ReturnNoContent()
        {
            var command = new CreateFeedbackTransactionCommand { ApprenticeshipId = 1159393, TemplateName = "ApprenticeshipOutcome" };
            _mockMediator
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreateFeedbackTransactionResponse());

            var result = await _controller.CreateFeedbackTransaction(command);

            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task And_CreateFeedbackTransactionNotFound_Then_ReturnNotFound()
        {
            var command = new CreateFeedbackTransactionCommand { ApprenticeshipId = 1, TemplateName = "Unknown" };
            _mockMediator
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreateFeedbackTransactionResponse { NotFound = true });

            var result = await _controller.CreateFeedbackTransaction(command);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test, MoqAutoData]
        public async Task And_GenerateCommandIsProcessedSuccessfully_Then_ReturnResults()
        {
            var result = await _controller.GenerateEmailTransactions();
            result.Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task And_ProcessCommandIsProcessedSuccessfully_Then_ReturnResults(
            long feedbackTransactionId,
            FeedbackTransaction feedbackTransaction,
            GetApprenticeResult mediatorApprenticeResult,
            ProcessEmailTransactionResponse mediatorCommandResponse,
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] FeedbackTransactionController controller)
        {
            mediatorApprenticeResult.ApprenticePreferences.Add(new InnerApi.Responses.ApprenticePreferenceDto { PreferenceId = 1, Status = true });

            mediatorMock.Setup(mediator =>
                mediator.Send(
                    It.Is<GetApprenticeQuery>(x => x.ApprenticeId == feedbackTransaction.ApprenticeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorApprenticeResult);

            mediatorMock.Setup(mediator =>
                mediator.Send(
                    It.Is<ProcessEmailTransactionCommand>(x => x.FeedbackTransactionId == feedbackTransactionId
                    && x.ApprenticeName == mediatorApprenticeResult.FirstName 
                    && x.ApprenticeEmailAddress == mediatorApprenticeResult.Email
                    && x.IsFeedbackEmailContactAllowed == true
                    && x.IsEngagementEmailContactAllowed == true),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorCommandResponse);

            ObjectResult objectResult = await controller.ProcessEmailTransaction(feedbackTransactionId, feedbackTransaction) as ObjectResult;
            Assert.That(objectResult, Is.Not.Null);
            objectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            object result = objectResult.Value;
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof (ProcessEmailTransactionResult));
        }
    }
}
