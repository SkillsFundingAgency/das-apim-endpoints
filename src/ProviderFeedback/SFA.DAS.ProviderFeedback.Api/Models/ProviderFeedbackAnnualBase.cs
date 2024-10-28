namespace SFA.DAS.ProviderFeedback.Api.Models
{
    public class ProviderFeedbackAnnualBase
    {
        public int ProviderId { get; set; }
        public bool IsEmployerProvider { get; set; }
        public GetEmployerFeedbackAnnualResponse EmployerFeedback { get; set; }
        public GetApprenticeFeedbackAnnualResponse ApprenticeFeedback { get; set; }

        protected GetEmployerFeedbackAnnualResponse EmployerFeedbackAnnualResponse(SFA.DAS.ProviderFeedback.Application.InnerApi.Responses.GetEmployerFeedbackAnnualResponse employerFeedback)
        {
            if (employerFeedback == null || !employerFeedback.AnnualEmployerFeedbackDetails.Any())
            {
                return new GetEmployerFeedbackAnnualResponse
                {
                    AnnualEmployerFeedbackDetails = new List<GetEmployerFeedbackAnnualSummary>()
                };
            }

            var annualFeedbackDetails = employerFeedback.AnnualEmployerFeedbackDetails
                .Select(summary => new GetEmployerFeedbackAnnualSummary
                {
                    TimePeriod = summary.TimePeriod,
                    TotalEmployerResponses = summary.ReviewCount,
                    TotalFeedbackRating = summary.Stars,
                    FeedbackAttributes = summary.ProviderAttribute
                        .Where(attr => attr.Strength + attr.Weakness != 0)
                        .Select(c => (GetEmployerFeedbackAnnualAttributeItem)c).ToList()
                })
                .ToList();

            return new GetEmployerFeedbackAnnualResponse
            {
                AnnualEmployerFeedbackDetails = annualFeedbackDetails
            };
        }

        protected GetApprenticeFeedbackAnnualResponse ApprenticeFeedbackAnnualResponse(SFA.DAS.ProviderFeedback.Application.InnerApi.Responses.GetApprenticeFeedbackAnnualResponse apprenticeFeedback)
        {
            if (apprenticeFeedback == null || !apprenticeFeedback.AnnualApprenticeFeedbackDetails.Any())
            {
                return new GetApprenticeFeedbackAnnualResponse
                {
                    AnnualApprenticeFeedbackDetails = new List<GetApprenticeFeedbackAnnualSummary>()
                };
            }

            var annualFeedbackDetails = apprenticeFeedback.AnnualApprenticeFeedbackDetails
                .Select(summary => new GetApprenticeFeedbackAnnualSummary
                {
                    TimePeriod = summary.TimePeriod,
                    TotalApprenticeResponses = summary.ReviewCount,
                    TotalFeedbackRating = summary.Stars,
                    FeedbackAttributes = summary.ProviderAttribute
                        .Where(attr => attr.Agree + attr.Disagree != 0)
                        .Select(c => (GetApprenticeFeedbackAnnualAttributeItem)c).ToList()
                })
                .ToList();

            return new GetApprenticeFeedbackAnnualResponse
            {
                AnnualApprenticeFeedbackDetails = annualFeedbackDetails
            };
        }
    }
}