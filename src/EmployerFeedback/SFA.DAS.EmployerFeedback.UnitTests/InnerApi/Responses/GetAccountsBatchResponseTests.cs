using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Responses
{
    [TestFixture]
    public class GetAccountsBatchResponseTests
    {
        [Test]
        public void AccountIds_CanBeSetAndRetrieved()
        {
            var accountIds = new List<long> { 1, 2, 3, 4, 5 };
            var response = new GetAccountsBatchResponse
            {
                AccountIds = accountIds
            };

            Assert.That(response.AccountIds, Is.EqualTo(accountIds));
        }

        [Test]
        public void AccountIds_CanBeNull()
        {
            var response = new GetAccountsBatchResponse
            {
                AccountIds = null
            };

            Assert.That(response.AccountIds, Is.Null);
        }

        [Test]
        public void AccountIds_CanBeEmptyList()
        {
            var accountIds = new List<long>();
            var response = new GetAccountsBatchResponse
            {
                AccountIds = accountIds
            };

            Assert.That(response.AccountIds, Is.EqualTo(accountIds));
            Assert.That(response.AccountIds.Count, Is.EqualTo(0));
        }
    }
}