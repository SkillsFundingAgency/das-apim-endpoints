using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.SendEmails;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.SendEmails
{
    public class WhenCallingHandle
    {
        private readonly Fixture _fixture;

        public WhenCallingHandle()
        {
            _fixture = new Fixture();
        }

        [Test, MoqAutoData]
        public async Task Notifications_Service_Is_Called(
            SendEmailsCommand command,
            [Frozen] Mock<INotificationService> notificationsService,
            SendEmailsCommandHandler handler
            )
        {
            await handler.Handle(command, CancellationToken.None);

            notificationsService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Exactly(command.EmailDataList.Count));
        }
    }
}
