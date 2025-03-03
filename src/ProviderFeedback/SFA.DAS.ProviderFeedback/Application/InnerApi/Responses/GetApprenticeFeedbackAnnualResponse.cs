﻿namespace SFA.DAS.ProviderFeedback.Application.InnerApi.Responses
{
    public class GetApprenticeFeedbackAnnualResponse
    {
        public IEnumerable<GetApprenticeFeedbackAnnualSummary> AnnualApprenticeFeedbackDetails { get; set; }
    }
    public class GetApprenticeFeedbackAnnualSummary
    {
        public long Ukprn { get; set; }
        public int ReviewCount { get; set; }
        public int Stars { get; set; }
        public string TimePeriod { get; set; }

        public IEnumerable<GetApprenticeFeedbackAnnualAttributeItem> ProviderAttribute { get; set; }
    }
    public class GetApprenticeFeedbackAnnualAttributeItem
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public int Agree { get; set; }
        public int Disagree { get; set; }
    }
}
