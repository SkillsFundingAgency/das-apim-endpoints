using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Queries.GetAccountsBatch;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Queries.GetAccountsBatch
{
    [TestFixture]
    public class GetAccountsBatchQueryTests
    {
        [Test]
        public void Constructor_SetsBatchSizeCorrectly()
        {
            var batchSize = 10;
            var query = new GetAccountsBatchQuery(batchSize);

            Assert.That(query.BatchSize, Is.EqualTo(batchSize));
        }
    }
}