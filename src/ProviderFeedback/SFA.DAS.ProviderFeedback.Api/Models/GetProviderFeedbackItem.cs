using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedback;

namespace SFA.DAS.ProviderFeedback.Api.Models
{
    public class GetProviderFeedbackItem : ProviderFeedbackBase
    {
        public GetProviderFeedbackItem Map(GetProviderFeedbackResult source)
        {
            var employerFeedbackResponse = EmployerFeedbackResponse(source.ProviderStandard.EmployerFeedback);
            var apprenticeFeedbackResponse = ApprenticeFeedbackResponse(source.ProviderStandard.ApprenticeFeedback);

            return new GetProviderFeedbackItem
            {
                ProviderId = source.ProviderStandard.Ukprn,
                EmployerFeedback = employerFeedbackResponse,
                ApprenticeFeedback = apprenticeFeedbackResponse,
            };
        }
    }
}