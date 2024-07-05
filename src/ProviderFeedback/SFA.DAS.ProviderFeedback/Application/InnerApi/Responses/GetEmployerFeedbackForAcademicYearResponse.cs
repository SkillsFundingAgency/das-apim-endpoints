namespace SFA.DAS.ProviderFeedback.Application.InnerApi.Responses
{
    public class GetEmployerFeedbackForAcademicYearResponse
    {
        public long Ukprn { get; set; }
        public int Stars { get; set; }
        public int ReviewCount { get; set; }
        public string TimePeriod { get; set; }
        public IEnumerable<GetEmployerFeedbackForAcademicYearAttributeItem> ProviderAttribute { get; set; }
    }

    public class GetEmployerFeedbackForAcademicYearAttributeItem
    {
        public string Name { get; set; }
        public int Strength { get; set; }
        public int Weakness { get; set; }
    }
}
