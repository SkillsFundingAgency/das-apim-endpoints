using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Commands.SyncEmployerAccounts;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Commands.SyncEmployerAccounts
{
    [TestFixture]
    public class SyncEmployerAccountsCommandTests
    {
        [Test]
        public void CanInstantiate()
        {
            var command = new SyncEmployerAccountsCommand();
            Assert.That(command, Is.Not.Null);
        }
    }
}
