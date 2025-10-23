using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Responses
{
    [TestFixture]
    public class GetUpdatedEmployerAccountsResponseTests
    {
        [Test]
        public void CanSetAndGetProperties()
        {
            var updatedAccounts = new List<UpdatedEmployerAccounts>
            {
                new UpdatedEmployerAccounts { AccountId = 1, AccountName = "Test1" },
                new UpdatedEmployerAccounts { AccountId = 2, AccountName = "Test2" }
            };
            var response = new GetUpdatedEmployerAccountsResponse
            {
                Data = updatedAccounts,
                Page = 2,
                TotalPages = 5
            };
            Assert.That(response.Data, Is.EqualTo(updatedAccounts));
            Assert.That(response.Page, Is.EqualTo(2));
            Assert.That(response.TotalPages, Is.EqualTo(5));
        }
    }
}
