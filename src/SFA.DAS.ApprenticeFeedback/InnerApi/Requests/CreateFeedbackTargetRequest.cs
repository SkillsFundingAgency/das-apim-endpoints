using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class CreateFeedbackTargetRequest : IPostApiRequest
    {
        public string PostUrl => "api/add-apprentice";

        public object Data { get; set; }

        public CreateFeedbackTargetRequest(CreateFeedbackTargetData data)
        {
            Data = data;
        }
    }

    public class CreateFeedbackTargetData
    {
        // Apprentice Accounts Id
        public Guid ApprenticeId { get; set; }
        // Apprentice Commitments Apprentice Id
        public long ApprenticeshipId { get; set; }
        // Commitments Apprentice Id
        public long CommitmentApprenticeshipId { get; set; }
    }
}
