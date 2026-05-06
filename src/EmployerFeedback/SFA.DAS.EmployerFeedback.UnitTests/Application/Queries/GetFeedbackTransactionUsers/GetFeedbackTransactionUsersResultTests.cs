using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Queries.GetFeedbackTransactionUsers;
using SFA.DAS.EmployerFeedback.Models;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Queries.GetFeedbackTransactionUsers
{
    public class GetFeedbackTransactionUsersResultTests
    {
        [Test]
        public void Properties_CanBeSetAndRetrieved()
        {
            const long accountId = 123L;
            const string accountName = "Test Account";
            const string templateName = "Test Template";
            var users = new List<FeedbackTransactionUser>
            {
                new FeedbackTransactionUser { Name = "John Doe", Email = "john@example.com" },
                new FeedbackTransactionUser { Name = "Jane Smith", Email = "jane@example.com" }
            };

            var result = new GetFeedbackTransactionUsersResult
            {
                AccountId = accountId,
                AccountName = accountName,
                TemplateName = templateName,
                Users = users
            };

            Assert.That(result.AccountId, Is.EqualTo(accountId));
            Assert.That(result.AccountName, Is.EqualTo(accountName));
            Assert.That(result.TemplateName, Is.EqualTo(templateName));
            Assert.That(result.Users, Is.EqualTo(users));
        }
    }
}