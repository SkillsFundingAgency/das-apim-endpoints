namespace SFA.DAS.ProviderFeedback.Application.InnerApi.Responses
{
    public class GetProviderStandardForAcademicYearItem
    {
        public int Ukprn { get; set; }
        public GetEmployerFeedbackForAcademicYearResponse EmployerFeedback { get; set; }
        public GetApprenticeFeedbackForAcademicYearResponse ApprenticeFeedback { get; set; }
    }
}