using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Models;

namespace SFA.DAS.EmployerFeedback.UnitTests.Models
{
    [TestFixture]
    public class FeedbackTransactionUserTests
    {
        [Test]
        public void Can_Construct_And_Assign_Properties()
        {
            var name = "John Doe";
            var email = "john.doe@example.com";

            var user = new FeedbackTransactionUser
            {
                Name = name,
                Email = email
            };

            Assert.That(user.Name, Is.EqualTo(name));
            Assert.That(user.Email, Is.EqualTo(email));
        }
    }
}