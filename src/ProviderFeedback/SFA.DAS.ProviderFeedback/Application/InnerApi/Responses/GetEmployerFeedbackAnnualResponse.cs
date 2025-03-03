namespace SFA.DAS.ProviderFeedback.Application.InnerApi.Responses
{
    public class GetEmployerFeedbackAnnualResponse
    {
        public IEnumerable<GetEmployerFeedbackStarsAnnualSummaryDto> AnnualEmployerFeedbackDetails { get; set; }
    }
    public class GetEmployerFeedbackStarsAnnualSummaryDto
    {
        public long Ukprn { get; set; }
        public int Stars { get; set; }
        public int ReviewCount { get; set; }
        public string TimePeriod { get; set; }

        public IEnumerable<GetEmployerFeedbackAnnualAttributeItem> ProviderAttribute { get; set; }
    }
    public class GetEmployerFeedbackAnnualAttributeItem
    {
        public string Name { get; set; }
        public int Strength { get; set; }
        public int Weakness { get; set; }
    }
}
