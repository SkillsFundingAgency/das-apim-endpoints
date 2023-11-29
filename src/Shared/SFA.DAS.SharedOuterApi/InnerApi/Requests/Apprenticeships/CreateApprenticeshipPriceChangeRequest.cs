using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships
{
    public class CreateApprenticeshipPriceChangeRequest : IPostApiRequest
    {
        public Guid ApprenticeshipKey { get; set; }
        private CreateApprenticeshipPriceChangeRequestData RequestData { get; set; }
        public string PostUrl => $"{ApprenticeshipKey}/priceHistory";
        public object Data
        {
            get => RequestData;
            set => RequestData = (CreateApprenticeshipPriceChangeRequestData)value;
        }
    }

    public class CreateApprenticeshipPriceChangeRequestData
    {
        public long? Ukprn { get; set; }
        public long? EmployerId { get; set; }
        public string UserId { get; set; }
        public decimal? TrainingPrice { get; set; }
        public decimal? AssessmentPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public string Reason { get; set; }
        public DateTime EffectiveDateTime { get; set; }
    }
}