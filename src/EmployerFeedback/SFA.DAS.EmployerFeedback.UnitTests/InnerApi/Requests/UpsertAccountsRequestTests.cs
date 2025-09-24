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
            var accountsData = new AccountsData
            {
                Accounts = new List<UpsertAccountsData>
                {
                    new UpsertAccountsData { AccountId = 1, AccountName = "Test1" },
                    new UpsertAccountsData { AccountId = 2, AccountName = "Test2" }
                }
            };
            var request = new UpsertAccountsRequest(accountsData);
            Assert.That(request.Data, Is.EqualTo(accountsData));
            Assert.That(request.PostUrl, Is.EqualTo("api/accounts"));
        }
    }
}
