using SFA.DAS.Recruit.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.Queries.GetApplicationReviewsCountByUkprn
{
    public record GetApplicationReviewsCountByUkprnResult
    {
        public List<ApplicationReviewStats> ApplicationReviewStatsList { get; init; } = [];
    }
}
