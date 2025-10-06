using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Queries.GetAccountsBatch;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Queries.GetAccountsBatch
{
    [TestFixture]
    public class GetAccountsBatchResultTests
    {
        [Test]
        public void AccountIds_CanBeSetAndRetrieved()
        {
            var accountIds = new List<long> { 1, 2, 3, 4, 5 };
            var result = new GetAccountsBatchResult
            {
                AccountIds = accountIds
            };

            Assert.That(result.AccountIds, Is.EqualTo(accountIds));
        }

        [Test]
        public void AccountIds_CanBeNull()
        {
            var result = new GetAccountsBatchResult
            {
                AccountIds = null
            };

            Assert.That(result.AccountIds, Is.Null);
        }

        [Test]
        public void AccountIds_CanBeEmptyList()
        {
            var accountIds = new List<long>();
            var result = new GetAccountsBatchResult
            {
                AccountIds = accountIds
            };

            Assert.That(result.AccountIds, Is.EqualTo(accountIds));
            Assert.That(result.AccountIds.Count, Is.EqualTo(0));
        }
    }
}