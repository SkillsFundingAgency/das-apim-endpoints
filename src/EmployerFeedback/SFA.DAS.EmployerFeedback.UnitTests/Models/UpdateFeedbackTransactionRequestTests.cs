using System;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Models;

namespace SFA.DAS.EmployerFeedback.UnitTests.Models
{
    [TestFixture]
    public class UpdateFeedbackTransactionRequestTests
    {
        [Test]
        public void Can_Construct_And_Assign_Properties()
        {
            var templateId = Guid.NewGuid();
            var sentCount = 1;
            var sentDate = DateTime.UtcNow;

            var request = new UpdateFeedbackTransactionRequest
            {
                TemplateId = templateId,
                SentCount = sentCount,
                SentDate = sentDate
            };

            Assert.That(request.TemplateId, Is.EqualTo(templateId));
            Assert.That(request.SentCount, Is.EqualTo(sentCount));
            Assert.That(request.SentDate, Is.EqualTo(sentDate));
        }
    }
}
