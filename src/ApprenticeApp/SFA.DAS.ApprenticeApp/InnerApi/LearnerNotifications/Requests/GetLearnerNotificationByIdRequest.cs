using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeApp.InnerApi.LearnerNotifications.Requests
{
    public class GetLearnerNotificationByIdRequest : IGetApiRequest
    {
        private readonly Guid _accountIdentifier;
        private readonly long _notificationIdentifier;

        public GetLearnerNotificationByIdRequest(Guid accountIdentifier, long notificationIdentifier)
        {
            _accountIdentifier = accountIdentifier;
            _notificationIdentifier = notificationIdentifier;
        }

        public string GetUrl => $"learner/{_accountIdentifier}/notifications/{_notificationIdentifier}";
    }
}