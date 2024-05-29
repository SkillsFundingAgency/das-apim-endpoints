namespace SFA.DAS.ProviderFeedback.Application.InnerApi.Responses
{
    public class GetProviderStandardItem
    {
        public int Ukprn { get; set; }
        public GetEmployerFeedbackResponse EmployerFeedback { get; set; }
        public GetApprenticeFeedbackResponse ApprenticeFeedback { get; set; }
    }
}