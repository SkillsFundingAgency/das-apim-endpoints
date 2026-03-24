using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeApp.InnerApi.LearnerNotifications.Requests
{
    public class GetLearnerNotificationsRequest : IGetApiRequest
    {
        private readonly Guid _accountIdentifier;

        public GetLearnerNotificationsRequest(Guid accountIdentifier)
        {
            _accountIdentifier = accountIdentifier;
        }

        public string GetUrl => $"learner/{_accountIdentifier}";
    }
}