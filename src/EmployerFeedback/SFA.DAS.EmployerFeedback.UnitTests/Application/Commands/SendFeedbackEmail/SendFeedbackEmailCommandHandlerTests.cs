using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Commands.SendFeedbackEmail;
using SFA.DAS.EmployerFeedback.Models;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Commands.SendFeedbackEmail
{
    [TestFixture]
    public class SendFeedbackEmailCommandHandlerTests
    {
        private Mock<INotificationService> _mockNotificationService;
        private Mock<ILogger<SendFeedbackEmailCommandHandler>> _mockLogger;
        private SendFeedbackEmailCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockNotificationService = new Mock<INotificationService>();
            _mockLogger = new Mock<ILogger<SendFeedbackEmailCommandHandler>>();

            _handler = new SendFeedbackEmailCommandHandler(
                _mockNotificationService.Object,
                _mockLogger.Object);
        }

        [Test]
        public async Task Then_SendsEmailWithCorrectParameters()
        {
            var request = new SendFeedbackEmailRequest
            {
                TemplateId = Guid.NewGuid(),
                Contact = "John Smith",
                Email = "john.smith@test.com",
                EmployerName = "Test Company Ltd",
                AccountHashedId = "ABC123",
                AccountsBaseUrl = "https://accounts.test.gov.uk",
                FeedbackBaseUrl = "https://feedback.test.gov.uk"
            };
            var command = new SendFeedbackEmailCommand(request);

            await _handler.Handle(command, CancellationToken.None);

            _mockNotificationService.Verify(x => x.Send(
                It.Is<SendEmailCommand>(emailCmd =>
                    emailCmd.TemplateId == request.TemplateId.ToString() &&
                    emailCmd.RecipientsAddress == request.Email &&
                    emailCmd.Tokens.ContainsKey("contact") &&
                    emailCmd.Tokens["contact"] == request.Contact &&
                    emailCmd.Tokens.ContainsKey("employername") &&
                    emailCmd.Tokens["employername"] == request.EmployerName &&
                    emailCmd.Tokens.ContainsKey("accounthashedid") &&
                    emailCmd.Tokens["accounthashedid"] == request.AccountHashedId &&
                    emailCmd.Tokens.ContainsKey("accountsbaseurl") &&
                    emailCmd.Tokens["accountsbaseurl"] == request.AccountsBaseUrl &&
                    emailCmd.Tokens.ContainsKey("feedbackbaseurl") &&
                    emailCmd.Tokens["feedbackbaseurl"] == request.FeedbackBaseUrl
                )), Times.Once);
        }
    }
}