﻿namespace SFA.DAS.Apprenticeships.Application.Notifications
{
    /// <summary>
    /// Notifications are sent via service bus events and do not return any data, this response will only
    /// return success or failure
    /// </summary>
    public class NotificationResponse
    {
        /// <summary>
        /// This indicates the success of the handler not that a message was sent. In some circumstances the handler
        /// may resolve to not send a message (e.g. if the user has opted out) but the handler completed without error
        /// </summary>
        public bool Success { get; set; }

        public static NotificationResponse Ok()
        {
            return new NotificationResponse { Success = true };
        }

        public static NotificationResponse Fail(string reason)
        {
            return new NotificationResponse { Success = false };
        }
    }
}
