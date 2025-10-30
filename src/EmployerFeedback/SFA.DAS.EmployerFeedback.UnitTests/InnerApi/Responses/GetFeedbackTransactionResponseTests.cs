using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;
using System;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Responses
{
    [TestFixture]
    public class GetFeedbackTransactionResponseTests
    {
        [Test]
        public void Then_SetsPropertiesCorrectly()
        {
            var id = 12345L;
            var accountId = 67890L;
            var accountName = "Test Company Ltd";
            var templateName = "FeedbackTemplate";
            var sendAfter = DateTime.UtcNow.AddDays(-1);
            var sentDate = DateTime.UtcNow;

            var response = new GetFeedbackTransactionResponse
            {
                Id = id,
                AccountId = accountId,
                AccountName = accountName,
                TemplateName = templateName,
                SendAfter = sendAfter,
                SentDate = sentDate
            };

            Assert.That(response.Id, Is.EqualTo(id));
            Assert.That(response.AccountId, Is.EqualTo(accountId));
            Assert.That(response.AccountName, Is.EqualTo(accountName));
            Assert.That(response.TemplateName, Is.EqualTo(templateName));
            Assert.That(response.SendAfter, Is.EqualTo(sendAfter));
            Assert.That(response.SentDate, Is.EqualTo(sentDate));
        }
    }
}