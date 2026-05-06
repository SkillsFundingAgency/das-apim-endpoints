using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Responses
{
    [TestFixture]
    public class GetFeedbackTransactionsBatchResponseTests
    {
        [Test]
        public void FeedbackTransactions_CanBeSet()
        {
            var feedbackTransactions = new List<long> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var response = new GetFeedbackTransactionsBatchResponse();

            response.FeedbackTransactions = feedbackTransactions;

            Assert.That(response.FeedbackTransactions, Is.EqualTo(feedbackTransactions));
        }
    }
}