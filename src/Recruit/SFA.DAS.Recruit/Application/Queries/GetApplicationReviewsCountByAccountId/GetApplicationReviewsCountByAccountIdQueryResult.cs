using SFA.DAS.Recruit.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.Queries.GetApplicationReviewsCountByAccountId
{
    public record GetApplicationReviewsCountByAccountIdQueryResult
    {
        public List<ApplicationReviewStats> ApplicationReviewStatsList { get; set; }

        public static implicit operator GetApplicationReviewsCountByAccountIdQueryResult(List<ApplicationReviewStats> response)
        {
            return new GetApplicationReviewsCountByAccountIdQueryResult
            {
                ApplicationReviewStatsList = response
            };
        }
    }
}