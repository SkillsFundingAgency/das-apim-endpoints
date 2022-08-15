﻿
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Api.Controllers;
using SFA.DAS.ApprenticeFeedback.Application.Commands.GenerateEmailTransaction;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.SharedOuterApi.Models;
using System.Threading;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprentice;
using SFA.DAS.ApprenticeFeedback.Application.Commands.ProcessEmailTransaction;


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
            long apprenticeFeedbackTransactionId,
            ApprenticeFeedbackTransaction feedbackTransaction,
            object mediatorTransactionResult,
            GetApprenticeResult mediatorApprenticeResult,
            [Frozen] Mock<IMediator> mockTransactionMediator,
            [Frozen] Mock<IMediator> mockApprenticeMediator,
            [Greedy] FeedbackTransactionController controller)
        {
            mockApprenticeMediator.Setup(mediator =>
                mediator.Send(
                    It.Is<GetApprenticeQuery>(x => x.ApprenticeId == feedbackTransaction.ApprenticeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorApprenticeResult);

            mockTransactionMediator.Setup(mediator =>
                mediator.Send(
                    It.Is<long>(x => x == apprenticeFeedbackTransactionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorTransactionResult);

            ObjectResult objectResult = await controller.ProcessEmailTransaction(apprenticeFeedbackTransactionId, feedbackTransaction) as ObjectResult;
            Assert.IsNotNull(objectResult);
            objectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            object result = objectResult.Value;
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof (ProcessEmailTransactionResult));
        }
    }
}
