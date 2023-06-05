using System;
using SFA.DAS.ApprenticePortal.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests
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
