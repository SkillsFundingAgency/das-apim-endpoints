using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Commands.TriggerFeedbackEmails;
using SFA.DAS.EmployerFeedback.Models;
using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Commands.TriggerFeedbackEmails
{
    [TestFixture]
    public class TriggerFeedbackEmailsCommandTests
    {
        [Test]
        public void Then_SetsPropertiesCorrectly_WhenConstructed()
        {
            var feedbackTransactionId = 12345L;
            var templateId = Guid.NewGuid();
            var request = new TriggerFeedbackEmailsRequest
            {
                NotificationTemplates = new List<NotificationTemplateRequest>
                {
                    new NotificationTemplateRequest
                    {
                        TemplateName = "TestTemplate",
                        TemplateId = templateId
                    }
                },
                EmployerAccountsBaseUrl = "https://employer-accounts.test"
            };

            var command = new TriggerFeedbackEmailsCommand(feedbackTransactionId, request);

            Assert.That(command.FeedbackTransactionId, Is.EqualTo(feedbackTransactionId));
            Assert.That(command.Request, Is.EqualTo(request));
            Assert.That(command.Request.NotificationTemplates.Count, Is.EqualTo(1));
            Assert.That(command.Request.NotificationTemplates[0].TemplateName, Is.EqualTo("TestTemplate"));
            Assert.That(command.Request.NotificationTemplates[0].TemplateId, Is.EqualTo(templateId));
            Assert.That(command.Request.EmployerAccountsBaseUrl, Is.EqualTo("https://employer-accounts.test"));
        }
    }
}