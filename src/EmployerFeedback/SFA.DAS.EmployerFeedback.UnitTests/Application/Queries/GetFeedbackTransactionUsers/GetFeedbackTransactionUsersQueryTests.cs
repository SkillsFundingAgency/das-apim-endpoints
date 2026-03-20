using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Queries.GetFeedbackTransactionUsers;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Queries.GetFeedbackTransactionUsers
{
    public class GetFeedbackTransactionUsersQueryTests
    {
        [Test]
        public void Constructor_SetsProperties()
        {
            const long feedbackTransactionId = 123L;

            var query = new GetFeedbackTransactionUsersQuery(feedbackTransactionId);

            Assert.That(query.FeedbackTransactionId, Is.EqualTo(feedbackTransactionId));
        }
    }
}