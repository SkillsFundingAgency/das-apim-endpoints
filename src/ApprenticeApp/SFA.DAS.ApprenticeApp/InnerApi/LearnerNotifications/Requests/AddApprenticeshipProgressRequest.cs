using SFA.DAS.Apim.Shared.Interfaces;
using System;

namespace SFA.DAS.ApprenticeApp.InnerApi.LearnerNotifications.Requests
{
    internal class AddApprenticeshipProgressRequest : IPostApiRequest
    {
        private readonly Guid _apprenticeId;

        public AddApprenticeshipProgressRequest(Guid apprenticeId)
        {
            _apprenticeId = apprenticeId;
        }

        public string PostUrl => $"notifications/apprenticeship-progress/{_apprenticeId}";
        public object Data { get; set; } = new { };
    }
}
