using System;

namespace SFA.DAS.ApprenticeApp.Application.Events
{
    public class ApprenticeSubscriptionCreateEvent
    {
        public Guid ApprenticeId { get; set; }
        public string Endpoint { get; set; }
        public string PublicKey { get; set; }
        public string AuthenticationSecret { get; set; }
    }
}