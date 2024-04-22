using System;

namespace SFA.DAS.ApprenticeApp.Application.Events
{
    public class ApprenticeSubscriptionDeleteEvent
    {
        public Guid ApprenticeId { get; set; }
    }
}