using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public record GetApplicationReviewsCountByAccountIdApiRequest(long AccountId, List<long> VacancyReferences) : IPostApiRequest
    {
        public string PostUrl => $"api/employer/{AccountId}/applicationReviews/count";
        public object Data { get; set; } = VacancyReferences;
    }
}