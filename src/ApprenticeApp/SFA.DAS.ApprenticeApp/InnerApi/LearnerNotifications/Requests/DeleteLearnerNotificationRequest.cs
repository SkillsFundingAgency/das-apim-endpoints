using SFA.DAS.Apim.Shared.Interfaces;
using System;

namespace SFA.DAS.ApprenticeApp.InnerApi.LearnerNotifications.Requests
{
    public class DeleteLearnerNotificationRequest : IDeleteApiRequest
    {
        private readonly Guid _accountIdentifier;
        private readonly long _notificationIdentifier;

        public DeleteLearnerNotificationRequest(
            Guid accountIdentifier,
            long notificationIdentifier)
        {
            _accountIdentifier = accountIdentifier;
            _notificationIdentifier = notificationIdentifier;
        }

        public string DeleteUrl =>
            $"learner/{_accountIdentifier}/notifications/{_notificationIdentifier}";
    }
}