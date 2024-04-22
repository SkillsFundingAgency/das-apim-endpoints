using System;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests
{
    public class PostApprenticeshipRequest : IPostApiRequest<MyApprenticeshipData>
    {
        private readonly Guid _apprenticeId;
        public string PostUrl => $"/apprentices/{_apprenticeId}/myapprenticeship";
        public MyApprenticeshipData Data { get; set; }

        public PostApprenticeshipRequest(Guid apprenticeId)
        {
            _apprenticeId = apprenticeId;
        }
    }
}
