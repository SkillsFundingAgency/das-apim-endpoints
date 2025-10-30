using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Models;
using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFeedback.UnitTests.Models
{
    [TestFixture]
    public class SendFeedbackEmailsRequestTests
    {
        [Test]
        public void Then_SetsNotificationTemplates_WhenProvided()
        {
            var templates = new List<NotificationTemplateRequest>
            {
                new NotificationTemplateRequest
                {
                    TemplateName = "Template1",
                    TemplateId = Guid.NewGuid()
                },
                new NotificationTemplateRequest
                {
                    TemplateName = "Template2",
                    TemplateId = Guid.NewGuid()
                }
            };

            var request = new SendFeedbackEmailsRequest
            {
                NotificationTemplates = templates,
                EmployerAccountsBaseUrl = "https://test.com"
            };

            Assert.That(request.NotificationTemplates, Is.EqualTo(templates));
            Assert.That(request.NotificationTemplates.Count, Is.EqualTo(2));
            Assert.That(request.EmployerAccountsBaseUrl, Is.EqualTo("https://test.com"));
        }
    }

    [TestFixture]
    public class NotificationTemplateRequestTests
    {
        [Test]
        public void Then_SetsPropertiesCorrectly()
        {
            var templateName = "TestTemplate";
            var templateId = Guid.NewGuid();

            var template = new NotificationTemplateRequest
            {
                TemplateName = templateName,
                TemplateId = templateId
            };

            Assert.That(template.TemplateName, Is.EqualTo(templateName));
            Assert.That(template.TemplateId, Is.EqualTo(templateId));
        }
    }
}