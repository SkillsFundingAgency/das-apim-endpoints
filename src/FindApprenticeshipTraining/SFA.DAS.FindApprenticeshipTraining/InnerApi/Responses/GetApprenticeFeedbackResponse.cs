using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetApprenticeFeedbackResponse
    {
        public long Ukprn { get; set; }
        public IEnumerable<GetApprenticeFeedbackRatingItem> ProviderRating { get; set; }
        public IEnumerable<GetApprenticeFeedbackAttributeItem> ProviderAttribute { get; set; }
    }

    public class GetApprenticeFeedbackAttributeItem
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public int Agree { get; set; }
        public int Disagree { get; set; }
    }

    public class GetApprenticeFeedbackRatingItem
    {
        public string Rating { get; set; }
        public int Count { get; set; }
    }
}
