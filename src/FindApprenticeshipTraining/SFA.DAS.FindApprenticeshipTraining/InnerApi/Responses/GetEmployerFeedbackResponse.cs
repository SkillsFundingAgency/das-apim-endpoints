using Newtonsoft.Json;
using SFA.DAS.FindApprenticeshipTraining.Application;
using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetEmployerFeedbackResponse
    {
        public long Ukprn { get; set; }
        public int Stars { get; set; }
        public int ReviewCount { get; set; }
        public IEnumerable<GetEmployerFeedbackResponseDetailItem> ProviderAttribute { get; set; }

        public static implicit operator GetEmployerFeedbackResponse(GetEmployerFeedbackSummaryItem item)
        {
            if(item == null)
                return null;

            return new GetEmployerFeedbackResponse
            {
                Ukprn = item.Ukprn,
                Stars = item.Stars,
                ReviewCount = item.ReviewCount,
                ProviderAttribute = new List<GetEmployerFeedbackResponseDetailItem>()
            };
        }
    }

    public class GetEmployerFeedbackResponseDetailItem
    {
        public string Name { get; set; }
        public int Strength { get; set; }
        public int Weakness { get; set; }
    }
}
