namespace SFA.DAS.ProviderFeedback.Api.Models
{
    public class GetEmployerFeedbackForAcademicYearResponse
    {
        public int TotalEmployerResponses { get; set; }
        public int TotalFeedbackRating { get; set; }
        public string TimePeriod { get; set; }
        public IEnumerable<GetEmployerFeedbackForAcademicYearAttributeItem> FeedbackAttributes { get; set; }
    }
    public class GetEmployerFeedbackForAcademicYearAttributeItem
    {
        public int Weakness { get; set; }
        public int Strength { get; set; }
        public string AttributeName { get; set; }
        public int TotalVotes { get; set; }
        public int Rating { get; set; }

        public static implicit operator GetEmployerFeedbackForAcademicYearAttributeItem(Application.InnerApi.Responses.GetEmployerFeedbackForAcademicYearAttributeItem source)
        {
            return new GetEmployerFeedbackForAcademicYearAttributeItem
            {
                AttributeName = source.Name,
                Strength = source.Strength,
                Weakness = source.Weakness,
                Rating = source.Strength - source.Weakness,
                TotalVotes = source.Strength + source.Weakness
            };
        }
    }
}