using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackForAcademicYear;

namespace SFA.DAS.ProviderFeedback.Api.Models
{
    public class GetProviderFeedbackForAcademicYearItem : ProviderFeedbackForAcademicYearBase
    {
        public GetProviderFeedbackForAcademicYearItem Map(GetProviderFeedbackForAcademicYearResult source)
        {
            var employerFeedbackResponse = EmployerFeedbackForAcademicYearResponse(source.ProviderStandard.EmployerFeedback);
            var apprenticeFeedbackResponse = ApprenticeFeedbackForAcademicYearResponse(source.ProviderStandard.ApprenticeFeedback);

            return new GetProviderFeedbackForAcademicYearItem
            {
                ProviderId = source.ProviderStandard.Ukprn,
                EmployerFeedback = employerFeedbackResponse,
                ApprenticeFeedback = apprenticeFeedbackResponse,
            };
        }
    }
}