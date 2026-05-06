using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Commands.GenerateFeedbackSummaries;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Commands.GenerateFeedbackSummaries
{
    [TestFixture]
    public class GenerateFeedbackSummariesCommandTests
    {
        [Test]
        public void Constructor_CreatesInstance()
        {
            var command = new GenerateFeedbackSummariesCommand();
            Assert.That(command, Is.Not.Null);
        }
    }
}
