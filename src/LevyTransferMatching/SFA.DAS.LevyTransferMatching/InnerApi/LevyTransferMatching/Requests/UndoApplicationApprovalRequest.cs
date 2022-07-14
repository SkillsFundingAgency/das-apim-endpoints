using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class UndoApplicationApprovalRequest : IPostApiRequest
    {
        private readonly long _pledgeId;
        private readonly int _applicationId;

        public UndoApplicationApprovalRequest(int pledgeId, int applicationId)
        {
            _pledgeId = pledgeId;
            _applicationId = applicationId;
            Data = null;
        }

        public string PostUrl => $"pledges/{_pledgeId}/applications/{_applicationId}/undo-approval";

        public object Data { get; set; }
    }
}
