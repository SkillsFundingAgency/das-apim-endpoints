using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests
{
    public class ProcessApprenticeshipApprovalRequest : IPostApiRequest
    {
        public long ApprenticeshipId { get; }
        public string PostUrl => $"api/apprenticeships/{ApprenticeshipId}/approvals";
        public object Data { get; set; }

        public ProcessApprenticeshipApprovalRequest(long apprenticeshipId, Body body)
        {
            ApprenticeshipId = apprenticeshipId;
            Data = body;
        }

        public class Body : SaveDataRequest
        {
            public bool ApplyChanges { get; set; }
        }
    }
}
