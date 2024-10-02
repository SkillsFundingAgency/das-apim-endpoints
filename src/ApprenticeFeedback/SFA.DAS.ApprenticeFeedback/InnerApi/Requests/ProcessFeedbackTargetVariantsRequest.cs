using SFA.DAS.ApprenticeFeedback.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class ProcessFeedbackTargetVariantsRequest : IPostApiRequest
    {
       
        public string PostUrl => $"api/FeedbackTargetVariant/process-variants";

        public object Data { get; set; }

        public ProcessFeedbackTargetVariantsRequest(ProcessFeedbackTargetVariantsData data)
        {
            Data = data;
        }
    }

    public class ProcessFeedbackTargetVariantsData
    {
        public List<FeedbackTargetVariant> FeedbackTargetVariants { get; set; }
        public ProcessFeedbackTargetVariantsData(List<FeedbackTargetVariant> variants)
        {
            FeedbackTargetVariants = variants;
        }
    }
}
