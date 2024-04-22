namespace SFA.DAS.ProviderFeedback.Api.Models
{
    public class ProviderFeedbackBase
    {
        public int ProviderId { get; set; }
        public GetEmployerFeedbackResponse EmployerFeedback { get; set; }
        public GetApprenticeFeedbackResponse ApprenticeFeedback { get; set; }

        protected GetEmployerFeedbackResponse EmployerFeedbackResponse(Application.InnerApi.Responses.GetEmployerFeedbackResponse employerFeedback)
        {
            if (employerFeedback == null || employerFeedback.ReviewCount == 0)
            {
                return new GetEmployerFeedbackResponse
                {
                    TotalEmployerResponses = 0,
                    TotalFeedbackRating = 0,
                    FeedbackAttributes = new List<GetEmployerFeedbackAttributeItem>()
                };
            }

            IEnumerable<GetEmployerFeedbackAttributeItem> feedbackAttrItems;

            if (employerFeedback?.ProviderAttribute == null)
            {
                feedbackAttrItems = new List<GetEmployerFeedbackAttributeItem>();
            }
            else
            {
                feedbackAttrItems = employerFeedback.ProviderAttribute
                    .Where(c => c.Strength + c.Weakness != 0)
                    .Select(c => (GetEmployerFeedbackAttributeItem)c).ToList();
            }

            return new GetEmployerFeedbackResponse
            {
                TotalFeedbackRating = employerFeedback.Stars,
                TotalEmployerResponses = employerFeedback.ReviewCount,
                FeedbackAttributes = feedbackAttrItems,
            };
        }

        protected GetApprenticeFeedbackResponse ApprenticeFeedbackResponse(
            Application.InnerApi.Responses.GetApprenticeFeedbackResponse apprenticeFeedback)
        {
            if (apprenticeFeedback == null || apprenticeFeedback.ReviewCount == 0)
            {
                return new GetApprenticeFeedbackResponse
                {
                    TotalApprenticeResponses = 0,
                    TotalFeedbackRating = 0,
                    FeedbackAttributes = new List<GetApprenticeFeedbackAttributeItem>(),
                };
            }

            IEnumerable<GetApprenticeFeedbackAttributeItem> feedbackAttrItems;
            if (apprenticeFeedback?.ProviderAttribute == null)
            {
                feedbackAttrItems = new List<GetApprenticeFeedbackAttributeItem>();
            }
            else
            {
                feedbackAttrItems = apprenticeFeedback.ProviderAttribute
                    .Where(c => c.Agree + c.Disagree != 0)
                    .Select(c => (GetApprenticeFeedbackAttributeItem)c).ToList();
            }

            return new GetApprenticeFeedbackResponse
            {
                TotalFeedbackRating = apprenticeFeedback.Stars,
                TotalApprenticeResponses = apprenticeFeedback.ReviewCount,
                FeedbackAttributes = feedbackAttrItems,
            };
        }
    }
}