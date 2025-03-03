namespace SFA.DAS.ProviderFeedback.Application.InnerApi.Responses
{
    public class GetProviderStandardAnnualItem
    {
        public int Ukprn { get; set; }
        public bool IsEmployerProvider { get; set; }
        public GetEmployerFeedbackAnnualResponse EmployerFeedback { get; set; }
        public GetApprenticeFeedbackAnnualResponse ApprenticeFeedback { get; set; }
    }
}