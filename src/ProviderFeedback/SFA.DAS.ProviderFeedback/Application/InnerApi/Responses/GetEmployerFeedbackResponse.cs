
namespace SFA.DAS.ProviderFeedback.Application.InnerApi.Responses
{
    public class GetEmployerFeedbackResponse
    {
        public long Ukprn { get; set; }
        public int Stars { get; set; }
        public int ReviewCount { get; set; }
        public IEnumerable<GetEmployerFeedbackAttributeItem> ProviderAttribute { get; set; }
    }

    public class GetEmployerFeedbackAttributeItem
    {
        public string Name { get; set; }
        public int Strength { get; set; }
        public int Weakness { get; set; }
    }
}
