using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Events;
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
            ApprenticeSubscriptionCreateEvent message = new() { ApprenticeId = Guid.Empty, AuthenticationSecret = "a", Endpoint = "b", PublicKey = "c" };

            await service.AddApprenticeSubscription(message);

            mockMessageSession.Verify(session => session.Publish(message, It.IsAny<PublishOptions>()));
        }

        [Test, MoqAutoData]
        public async Task Delete_Apprentice_Subscription_Sends_Event(
            [Frozen] Mock<IMessageSession> mockMessageSession,
            SubscriptionService service)
        {
            ApprenticeSubscriptionDeleteEvent message = new() { ApprenticeId = Guid.Empty};

            await service.DeleteApprenticeSubscription(message);

            mockMessageSession.Verify(session => session.Publish(message, It.IsAny<PublishOptions>()));
        }

    }
}