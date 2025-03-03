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
        public bool ClearStaging { get; set; }
        public bool MergeStaging { get; set; }
        public List<FeedbackTargetVariant> FeedbackTargetVariants { get; set; }

        public ProcessFeedbackTargetVariantsData(bool clearStaging, bool mergeStaging, List<FeedbackTargetVariant> variants)
        {
            ClearStaging = clearStaging;
            MergeStaging = mergeStaging;
            FeedbackTargetVariants = variants;
        }
    }
}
