﻿using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.NServiceBus.Services;
using SFA.DAS.SharedOuterApi.Models.Messages;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services
{
    public class WhenSendingAnEmail
    {
        [Test, MoqAutoData]
        public async Task Then_Sends_Correct_Message_To_Notification_Service(
            SendEmailCommand email,
            [Frozen] Mock<IMessageSession> mockMessageSession,
            NotificationService service)
        {
            await service.Send(email);

            mockMessageSession.Verify(session => session.Send(email, It.IsAny<SendOptions>()));
        }
    }

}