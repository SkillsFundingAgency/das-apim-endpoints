namespace SFA.DAS.ProviderFeedback.Api.Models
{
    public class ProviderFeedbackForAcademicYearBase
    {
        public int ProviderId { get; set; }
        public GetEmployerFeedbackForAcademicYearResponse EmployerFeedback { get; set; }
        public GetApprenticeFeedbackForAcademicYearResponse ApprenticeFeedback { get; set; }

        protected GetEmployerFeedbackForAcademicYearResponse EmployerFeedbackForAcademicYearResponse(Application.InnerApi.Responses.GetEmployerFeedbackForAcademicYearResponse employerFeedback)
        {
            if (employerFeedback == null || employerFeedback.ReviewCount == 0)
            {
                return new GetEmployerFeedbackForAcademicYearResponse
                {
                    TotalEmployerResponses = 0,
                    TotalFeedbackRating = 0,
                    FeedbackAttributes = new List<GetEmployerFeedbackForAcademicYearAttributeItem>()
                };
            }

            IEnumerable<GetEmployerFeedbackForAcademicYearAttributeItem> feedbackAttrItems;

            if (employerFeedback?.ProviderAttribute == null)
            {
                feedbackAttrItems = new List<GetEmployerFeedbackForAcademicYearAttributeItem>();
            }
            else
            {
                feedbackAttrItems = employerFeedback.ProviderAttribute
                    .Where(c => c.Strength + c.Weakness != 0)
                    .Select(c => (GetEmployerFeedbackForAcademicYearAttributeItem)c).ToList();
            }

            return new GetEmployerFeedbackForAcademicYearResponse
            {
                TotalFeedbackRating = employerFeedback.Stars,
                TotalEmployerResponses = employerFeedback.ReviewCount,
                TimePeriod = employerFeedback.TimePeriod,
                FeedbackAttributes = feedbackAttrItems,
            };
        }

        protected GetApprenticeFeedbackForAcademicYearResponse ApprenticeFeedbackForAcademicYearResponse(
            Application.InnerApi.Responses.GetApprenticeFeedbackForAcademicYearResponse apprenticeFeedback)
        {
            if (apprenticeFeedback == null || apprenticeFeedback.ReviewCount == 0)
            {
                return new GetApprenticeFeedbackForAcademicYearResponse  
                {
                    TotalApprenticeResponses = 0,
                    TotalFeedbackRating = 0,
                    FeedbackAttributes = new List<GetApprenticeFeedbackForAcademicYearAttributeItem>(),
                };
            }

            IEnumerable<GetApprenticeFeedbackForAcademicYearAttributeItem> feedbackAttrItems;
            if (apprenticeFeedback?.ProviderAttribute == null)
            {
                feedbackAttrItems = new List<GetApprenticeFeedbackForAcademicYearAttributeItem>();
            }
            else
            {
                feedbackAttrItems = apprenticeFeedback.ProviderAttribute
                    .Where(c => c.Agree + c.Disagree != 0)
                    .Select(c => (GetApprenticeFeedbackForAcademicYearAttributeItem)c).ToList();
            }

            return new GetApprenticeFeedbackForAcademicYearResponse
            {
                TotalFeedbackRating = apprenticeFeedback.Stars,
                TotalApprenticeResponses = apprenticeFeedback.ReviewCount,
                TimePeriod = apprenticeFeedback.TimePeriod,
                FeedbackAttributes = feedbackAttrItems,
            };
        }
    }
}