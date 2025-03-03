using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackAnnual;

namespace SFA.DAS.ProviderFeedback.Api.Models
{
    public class GetProviderFeedbackAnnualItem : ProviderFeedbackAnnualBase
    {
        public GetProviderFeedbackAnnualItem Map(GetProviderFeedbackAnnualResult source)
        {
            var employerFeedbackResponse = EmployerFeedbackAnnualResponse(source.ProviderStandard.EmployerFeedback);
            var apprenticeFeedbackResponse = ApprenticeFeedbackAnnualResponse(source.ProviderStandard.ApprenticeFeedback);

            return new GetProviderFeedbackAnnualItem
            {
                ProviderId = source.ProviderStandard.Ukprn,
                IsEmployerProvider = source.ProviderStandard.IsEmployerProvider,
                EmployerFeedback = employerFeedbackResponse,
                ApprenticeFeedback = apprenticeFeedbackResponse,
            };
        }
    }
}