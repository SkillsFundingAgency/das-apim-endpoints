using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PushNotifications.Messages.Commands
{
    [ExcludeFromCodeCoverage]
    public class RemoveWebPushSubscriptionCommand
    {
        public Guid ApprenticeId { get; set; }
        public required string Endpoint { get; set; }
    }
}
