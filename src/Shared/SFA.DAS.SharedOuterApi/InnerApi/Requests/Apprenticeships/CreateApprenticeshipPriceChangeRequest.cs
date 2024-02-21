using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships
{
    public class PostCreateApprenticeshipPriceChangeRequest : IPostApiRequest
    {
        public PostCreateApprenticeshipPriceChangeRequest(
            Guid apprenticeshipKey,
            long? providerId,
            long? employerId,
            string userId,
            decimal? trainingPrice,
            decimal? assessmentPrice,
            decimal totalPrice,
            string reason,
            DateTime effectiveFromDate)
        {
            ApprenticeshipKey = apprenticeshipKey;
            Data = new CreateApprenticeshipPriceChangeRequest
            {
                ProviderId = providerId,
                EmployerId = employerId,
                UserId = userId,
                TrainingPrice = trainingPrice,
                AssessmentPrice = assessmentPrice,
                TotalPrice = totalPrice,
                Reason = reason,
                EffectiveFromDate = effectiveFromDate
            };
        }
        
        public Guid ApprenticeshipKey { get; set; }
        public string PostUrl => $"{ApprenticeshipKey}/priceHistory";
        public object Data { get; set; }
    }

    public class CreateApprenticeshipPriceChangeRequest
    {
        public long? ProviderId { get; set; }
        public long? EmployerId { get; set; }
        public string UserId { get; set; }
        public decimal? TrainingPrice { get; set; }
        public decimal? AssessmentPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Reason { get; set; }
        public DateTime EffectiveFromDate { get; set; }
    }
}