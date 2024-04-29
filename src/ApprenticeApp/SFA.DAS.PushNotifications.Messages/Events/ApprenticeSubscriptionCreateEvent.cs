using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PushNotifications.Messages.Events
{
    [ExcludeFromCodeCoverage]
    public class ApprenticeSubscriptionCreateEvent
    {
        public Guid ApprenticeId { get; set; }
        public string Endpoint { get; set; } = null!;
        public string PublicKey { get; set; } = null!;
        public string AuthenticationSecret { get; set; } = null!;
    }
}
