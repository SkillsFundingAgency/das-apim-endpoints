using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using System;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class UpdateFeedbackTransactionRequestTests
    {
        [Test]
        public void Then_SetsCorrectUrl_WhenConstructed()
        {
            var feedbackTransactionId = 12345L;
            var data = new UpdateFeedbackTransactionData
            {
                TemplateId = Guid.NewGuid(),
                SentCount = 5,
                SentDate = DateTime.UtcNow
            };

            var request = new UpdateFeedbackTransactionRequest(feedbackTransactionId, data);

            Assert.That(request.PutUrl, Is.EqualTo("api/feedbacktransactions/12345"));
        }
    }

    [TestFixture]
    public class UpdateFeedbackTransactionDataTests
    {
        [Test]
        public void Then_SetsPropertiesCorrectly()
        {
            var templateId = Guid.NewGuid();
            var sentCount = 10;
            var sentDate = DateTime.UtcNow;

            var data = new UpdateFeedbackTransactionData
            {
                TemplateId = templateId,
                SentCount = sentCount,
                SentDate = sentDate
            };

            Assert.That(data.TemplateId, Is.EqualTo(templateId));
            Assert.That(data.SentCount, Is.EqualTo(sentCount));
            Assert.That(data.SentDate, Is.EqualTo(sentDate));
        }
    }
}