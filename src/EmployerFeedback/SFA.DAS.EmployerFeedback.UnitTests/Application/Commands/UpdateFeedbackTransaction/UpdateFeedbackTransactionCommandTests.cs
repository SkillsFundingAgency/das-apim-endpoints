using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Commands.UpdateFeedbackTransaction;
using SFA.DAS.EmployerFeedback.Models;
using System;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Commands.UpdateFeedbackTransaction
{
    [TestFixture]
    public class UpdateFeedbackTransactionCommandTests
    {
        [Test]
        public void Command_Has_Correct_Properties()
        {
            var id = 67890L;
            var templateId = Guid.NewGuid();
            var sentCount = 2;
            var sentDate = DateTime.UtcNow;

            var request = new UpdateFeedbackTransactionRequest
            {
                TemplateId = templateId,
                SentCount = sentCount,
                SentDate = sentDate
            };

            var command = new UpdateFeedbackTransactionCommand(id, request);

            Assert.That(command, Is.Not.Null);
            Assert.That(command.Id, Is.EqualTo(id));
            Assert.That(command.Request, Is.Not.Null);
            Assert.That(command.Request.TemplateId, Is.EqualTo(templateId));
            Assert.That(command.Request.SentCount, Is.EqualTo(sentCount));
            Assert.That(command.Request.SentDate, Is.EqualTo(sentDate));
        }
    }
}
