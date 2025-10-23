using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Commands.UpsertFeedbackTransaction;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Commands.UpsertFeedbackTransaction
{
    [TestFixture]
    public class UpsertFeedbackTransactionCommandTests
    {
        [Test]
        public void Constructor_SetsPropertiesCorrectly()
        {
            var command = new UpsertFeedbackTransactionCommand
            {
                AccountId = 12345L
            };

            Assert.That(command.AccountId, Is.EqualTo(12345L));
        }
    }
}