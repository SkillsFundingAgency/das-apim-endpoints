using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class CreateApprenticeFeedbackTargetRequest : IPostApiRequest
    {
        public string PostUrl => "api/apprenticefeedbacktarget";

        public object Data { get; set; }

        public CreateApprenticeFeedbackTargetRequest(CreateApprenticeFeedbackTargetData data)
        {
            Data = data;
        }
    }

    public class CreateApprenticeFeedbackTargetData
    {
        // Apprentice Accounts Id
        public Guid ApprenticeId { get; set; }
        // Apprentice Commitments Apprentice Id
        public long ApprenticeshipId { get; set; }
        // Commitments Apprentice Id
        public long CommitmentApprenticeshipId { get; set; }
    }
}
