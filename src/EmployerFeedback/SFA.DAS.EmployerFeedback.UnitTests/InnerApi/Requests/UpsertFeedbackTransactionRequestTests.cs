using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class UpsertFeedbackTransactionRequestTests
    {
        [Test]
        public void Constructor_SetsPropertiesCorrectly()
        {
            var accountId = 12345L;
            var data = new UpsertFeedbackTransactionData
            {
                Active = new List<ApprenticeshipStatusItem>
                {
                    new ApprenticeshipStatusItem { Ukprn = 10023829, CourseCode = "430" }
                }
            };

            var request = new UpsertFeedbackTransactionRequest(accountId, data);

            Assert.That(request.AccountId, Is.EqualTo(accountId));
            Assert.That(request.Data, Is.EqualTo(data));
        }

        [Test]
        public void PostUrl_ReturnsCorrectUrl()
        {
            var accountId = 12345L;
            var data = new UpsertFeedbackTransactionData();
            var request = new UpsertFeedbackTransactionRequest(accountId, data);

            var url = request.PostUrl;

            Assert.That(url, Is.EqualTo("api/accounts/12345/feedbacktransaction"));
        }
    }
}