using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PushNotifications.Messages.Events
{
    [ExcludeFromCodeCoverage]
    public class ApprenticeSubscriptionDeleteEvent
    {
        public Guid ApprenticeId { get; set; }
        public string EndPoint { get; set; } = null!;
    }
}
