using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Queries.GetEmailNudgeAccountsBatch;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Queries.GetEmailNudgeAccountsBatch
{
    [TestFixture]
    public class GetEmailNudgeAccountsBatchQueryTests
    {
        [Test]
        public void Constructor_SetsBatchSizeCorrectly()
        {
            var batchSize = 10;
            var query = new GetEmailNudgeAccountsBatchQuery(batchSize);

            Assert.That(query.BatchSize, Is.EqualTo(batchSize));
        }
    }
}