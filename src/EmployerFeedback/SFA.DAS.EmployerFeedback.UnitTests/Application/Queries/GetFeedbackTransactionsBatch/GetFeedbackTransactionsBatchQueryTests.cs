using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Queries.GetFeedbackTransactionsBatch;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Queries.GetFeedbackTransactionsBatch
{
    [TestFixture]
    public class GetFeedbackTransactionsBatchQueryTests
    {
        [Test]
        public void Constructor_SetsProperties_WhenCalled()
        {
            var batchSize = 10;

            var query = new GetFeedbackTransactionsBatchQuery(batchSize);

            Assert.That(query.BatchSize, Is.EqualTo(batchSize));
        }
    }
}