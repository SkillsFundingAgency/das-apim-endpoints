using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public record GetApplicationReviewsCountByUkprnApiRequest(int Ukprn, List<long> VacancyReferences) : IPostApiRequest
    {
        public string PostUrl => $"api/provider/{Ukprn}/applicationReviews/count";
        public object Data { get; set; } = VacancyReferences;
    }
}