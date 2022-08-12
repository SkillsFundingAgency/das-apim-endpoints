using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetApprenticeFeedbackResponse
    {
        public long Ukprn { get; set; }
        public int ReviewCount { get; set; }
        public int Stars { get; set; }
        
        public IEnumerable<GetApprenticeFeedbackAttributeItem> ProviderAttribute { get; set; }

        public static implicit operator GetApprenticeFeedbackResponse(GetApprenticeFeedbackSummaryItem item)
        {
            if(item == null)
            {
                return null;
            }

            return new GetApprenticeFeedbackResponse
            {
                Ukprn = item.Ukprn,
                Stars = item.Stars,
                ReviewCount = item.ReviewCount,
                ProviderAttribute = new List<GetApprenticeFeedbackAttributeItem>()
            };
        }
    }

    public class GetApprenticeFeedbackAttributeItem
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public int Agree { get; set; }
        public int Disagree { get; set; }
    }
}
