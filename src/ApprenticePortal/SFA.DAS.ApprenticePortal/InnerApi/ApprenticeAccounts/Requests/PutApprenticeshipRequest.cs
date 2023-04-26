using System;
using SFA.DAS.ApprenticePortal.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests
{
    public class PutApprenticeshipRequest : IPutApiRequest<MyApprenticeshipData>
    {
        private readonly Guid _apprenticeId;
        public string PutUrl => $"/apprentices/{_apprenticeId}/myapprenticeship";
        public MyApprenticeshipData Data { get; set; }

        public PutApprenticeshipRequest(Guid apprenticeId)
        {
            _apprenticeId = apprenticeId;
        }
    }
}
