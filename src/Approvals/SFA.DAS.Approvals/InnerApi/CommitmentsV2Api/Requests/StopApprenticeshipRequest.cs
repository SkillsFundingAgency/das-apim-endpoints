using System;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests
{
    public class StopApprenticeshipRequest : IPostApiRequest
    {
        public long ApprenticeshipId { get; }

        public StopApprenticeshipRequest(long apprenticeshipId, Body body)
        {
            ApprenticeshipId = apprenticeshipId;
            Data = body;
        }

        public string PostUrl => $"api/apprenticeships/{ApprenticeshipId}/stop";
        public object Data { get; set; }

        public class Body
        {
            public long AccountId { get; set; }
            public DateTime StopDate { get; set; }
            public bool MadeRedundant { get; set; }
            public UserInfo UserInfo { get; set; }
        }
    }
}
