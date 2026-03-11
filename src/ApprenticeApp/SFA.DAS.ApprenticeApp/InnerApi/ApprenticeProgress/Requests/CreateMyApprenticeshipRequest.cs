using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests
{
    public class CreateMyApprenticeshipRequest : IPostApiRequest
    {
        public Guid ApprenticeId { get; set; }
        public object Data { get; set; }

        public CreateMyApprenticeshipRequest(Guid apprenticeId, CreateMyApprenticeshipData data)
        {
            ApprenticeId = apprenticeId;
            Data = data;
        }

        public string PostUrl => $"/apprentices/{ApprenticeId}/MyApprenticeship";
    }
}
