using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Queries.GetFeedbackTransactionsBatch;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Queries.GetFeedbackTransactionsBatch
{
    [TestFixture]
    public class GetFeedbackTransactionsBatchResultTests
    {
        [Test]
        public void FeedbackTransactions_CanBeSet()
        {
            var feedbackTransactions = new List<long> { 1, 2, 3, 4, 5 };
            var result = new GetFeedbackTransactionsBatchResult();

            result.FeedbackTransactions = feedbackTransactions;

            Assert.That(result.FeedbackTransactions, Is.EqualTo(feedbackTransactions));
        }
    }
}