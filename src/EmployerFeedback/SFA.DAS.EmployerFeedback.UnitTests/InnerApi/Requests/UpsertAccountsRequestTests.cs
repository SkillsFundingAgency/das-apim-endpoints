using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class UpsertAccountsRequestTests
    {
        [Test]
        public void Constructor_SetsDataProperty()
        {
            var accounts = new List<UpsertAccountsData>
            {
                new UpsertAccountsData { AccountId = 1, AccountName = "Test1" },
                new UpsertAccountsData { AccountId = 2, AccountName = "Test2" }
            };
            var request = new UpsertAccountsRequest(accounts);
            Assert.That(request.Data, Is.EqualTo(accounts));
            Assert.That(request.PostUrl, Is.EqualTo("api/account"));
        }
    }
}
