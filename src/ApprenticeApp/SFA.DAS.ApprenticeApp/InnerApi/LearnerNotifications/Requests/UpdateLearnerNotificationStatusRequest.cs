using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeApp.InnerApi.LearnerNotifications.Requests
{
    public class UpdateLearnerNotificationStatusRequest : IPutApiRequest<UpdateNotificationStatusData>
    {
        private readonly Guid _accountIdentifier;
        private readonly long _notificationIdentifier;

        public UpdateLearnerNotificationStatusRequest(Guid accountIdentifier, long notificationIdentifier, UpdateNotificationStatusData data)
        {
            _accountIdentifier = accountIdentifier;
            _notificationIdentifier = notificationIdentifier;
            Data = data;
        }

        public string PutUrl => $"learner/{_accountIdentifier}/notifications/{_notificationIdentifier}/status";
        public UpdateNotificationStatusData Data { get; set; }
    }
}