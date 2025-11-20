using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Commands.SendFeedbackEmail;
using SFA.DAS.EmployerFeedback.Models;
using System;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Commands.SendFeedbackEmail
{
    [TestFixture]
    public class SendFeedbackEmailCommandTests
    {
        [Test]
        public void Then_InitializesWithRequest()
        {
            var request = new SendFeedbackEmailRequest
            {
                TemplateId = Guid.NewGuid(),
                Contact = "John Smith",
                Email = "john.smith@test.com",
                EmployerName = "Test Company Ltd",
                AccountHashedId = "ABC123",
                AccountsBaseUrl = "https://accounts.test.gov.uk"
            };

            var command = new SendFeedbackEmailCommand(request);

            Assert.That(command.Request, Is.EqualTo(request));
        }
    }
}