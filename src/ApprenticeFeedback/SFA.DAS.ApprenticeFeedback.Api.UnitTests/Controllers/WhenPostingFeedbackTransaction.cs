using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Api.Controllers;
using SFA.DAS.ApprenticeFeedback.Application.Commands.ProcessEmailTransaction;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprentice;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

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
        public async Task And_GenerateCommandIsProcessedSuccessfully_Then_ReturnResults()
        {
            var result = await _controller.GenerateEmailTransaction();
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
            Assert.IsNotNull(objectResult);
            objectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            object result = objectResult.Value;
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof (ProcessEmailTransactionResult));
        }
    }
}
