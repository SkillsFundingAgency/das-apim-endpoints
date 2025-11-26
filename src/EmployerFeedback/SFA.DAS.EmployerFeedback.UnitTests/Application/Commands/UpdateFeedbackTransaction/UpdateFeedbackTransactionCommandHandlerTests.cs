using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Commands.UpdateFeedbackTransaction;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Commands.UpdateFeedbackTransaction
{
    [TestFixture]
    public class UpdateFeedbackTransactionCommandHandlerTests
    {
        private Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> _mockApiClient;
        private Mock<ILogger<UpdateFeedbackTransactionCommandHandler>> _mockLogger;
        private UpdateFeedbackTransactionCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockApiClient = new Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>>();
            _mockLogger = new Mock<ILogger<UpdateFeedbackTransactionCommandHandler>>();
            _handler = new UpdateFeedbackTransactionCommandHandler(_mockApiClient.Object, _mockLogger.Object);
        }

        [Test]
        public async Task Handle_CallsApiClientWithCorrectParameters()
        {
            var id = 12345L;
            var templateId = Guid.NewGuid();
            var sentCount = 1;
            var sentDate = DateTime.UtcNow;

            var request = new EmployerFeedback.Models.UpdateFeedbackTransactionRequest
            {
                TemplateId = templateId,
                SentCount = sentCount,
                SentDate = sentDate
            };
            var command = new UpdateFeedbackTransactionCommand(id, request);

            _mockApiClient
                .Setup(x => x.Put(It.IsAny<UpdateFeedbackTransactionRequest>()))
                .Returns(Task.CompletedTask);

            await _handler.Handle(command, CancellationToken.None);

            _mockApiClient.Verify(x => x.Put(
                It.Is<UpdateFeedbackTransactionRequest>(r =>
                    r.FeedbackTransactionId == id &&
                    r.Data.TemplateId == request.TemplateId &&
                    r.Data.SentCount == request.SentCount &&
                    r.Data.SentDate == request.SentDate)),
                Times.Once);
        }

        [Test]
        public void Handle_ThrowsException_WhenApiCallFails()
        {
            var id = 11111L;
            var request = new EmployerFeedback.Models.UpdateFeedbackTransactionRequest
            {
                TemplateId = Guid.NewGuid(),
                SentCount = 1,
                SentDate = DateTime.UtcNow
            };
            var command = new UpdateFeedbackTransactionCommand(id, request);

            _mockApiClient
                .Setup(x => x.Put(It.IsAny<UpdateFeedbackTransactionRequest>()))
                .ThrowsAsync(new Exception("API call failed"));

            Assert.ThrowsAsync<Exception>(async () => await _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task Handle_CompletesSuccessfully_WhenApiCallSucceeds()
        {
            var id = 22222L;
            var request = new EmployerFeedback.Models.UpdateFeedbackTransactionRequest
            {
                TemplateId = Guid.NewGuid(),
                SentCount = 1,
                SentDate = DateTime.UtcNow
            };
            var command = new UpdateFeedbackTransactionCommand(id, request);

            _mockApiClient
                .Setup(x => x.Put(It.IsAny<UpdateFeedbackTransactionRequest>()))
                .Returns(Task.CompletedTask);

            await _handler.Handle(command, CancellationToken.None);

            _mockApiClient.Verify(x => x.Put(It.IsAny<UpdateFeedbackTransactionRequest>()), Times.Once);
        }
    }
}