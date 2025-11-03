using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Models;
using System;

namespace SFA.DAS.EmployerFeedback.UnitTests.Models
{

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