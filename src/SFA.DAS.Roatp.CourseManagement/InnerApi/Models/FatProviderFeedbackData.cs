using System.Collections.Generic;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Models
{
    public class FatProviderFeedbackData
    {
        public int Total { get; set; }
        public List<KeyValuePair<string, int>> FeedbackRating { get; set; }

        public List<ProviderAttribute> ProviderAttributes { get; set; }
    }
}