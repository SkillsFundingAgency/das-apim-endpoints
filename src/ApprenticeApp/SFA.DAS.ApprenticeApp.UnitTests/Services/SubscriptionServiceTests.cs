using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.PushNotifications.Messages.Commands;
using SFA.DAS.ApprenticeApp.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests
{
    public class SubscriptionServiceTests
    {
        [Test, MoqAutoData]
        public async Task Add_Apprentice_Subscription_Sends_Event(
            [Frozen] Mock<IMessageSession> mockMessageSession,
            SubscriptionService service)
        {
            AddWebPushSubscriptionCommand message = new() { ApprenticeId = Guid.Empty, AuthenticationSecret = "a", Endpoint = "b", PublicKey = "c" };

            await service.AddApprenticeSubscription(message);

            mockMessageSession.Verify(session => session.Send(message, It.IsAny<SendOptions>()));
        }

        [Test, MoqAutoData]
        public async Task Delete_Apprentice_Subscription_Sends_Event(
            [Frozen] Mock<IMessageSession> mockMessageSession,
            SubscriptionService service)
        {
            RemoveWebPushSubscriptionCommand message = new() { ApprenticeId = Guid.Empty, Endpoint = "endpoint"};

            await service.RemoveApprenticeSubscription(message);

            mockMessageSession.Verify(session => session.Send(message, It.IsAny<SendOptions>()));
        }
    }
}