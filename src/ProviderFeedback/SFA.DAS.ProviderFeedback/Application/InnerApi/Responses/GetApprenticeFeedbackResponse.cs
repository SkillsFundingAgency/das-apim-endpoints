namespace SFA.DAS.ProviderFeedback.Application.InnerApi.Responses
{
    public class GetApprenticeFeedbackResponse
    {
        public long Ukprn { get; set; }
        public int ReviewCount { get; set; }
        public int Stars { get; set; }

        public IEnumerable<GetApprenticeFeedbackAttributeItem> ProviderAttribute { get; set; }
    }

    public class GetApprenticeFeedbackAttributeItem
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public int Agree { get; set; }
        public int Disagree { get; set; }
    }
}
