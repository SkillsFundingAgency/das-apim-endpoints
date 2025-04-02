using System.Collections.Generic;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Application.Queries.GetApplicationReviewsCountByUkprn
{
    public record GetApplicationReviewsCountByUkprnQueryResult
    {
        public List<ApplicationReviewStats> ApplicationReviewStatsList { get; set; }

        public static implicit operator GetApplicationReviewsCountByUkprnQueryResult(List<ApplicationReviewStats> response)
        {
            return new GetApplicationReviewsCountByUkprnQueryResult
            {
                ApplicationReviewStatsList = response
            };
        }
    }
}