using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplicationsCount
{
    public record GetApplicationsCountQueryResult
    {
        public List<ApplicationStats> Stats { get; init; } = [];

        public static implicit operator GetApplicationsCountQueryResult(PostApplicationsCountApiResponse source)
        {
            return new GetApplicationsCountQueryResult
            {
                Stats = source.Stats.Select(x => (ApplicationStats)x).ToList()
            };
        }

        public record ApplicationStats
        {
            public string Status { get; set; }
            public int Count { get; set; }

            public static implicit operator ApplicationStats(PostApplicationsCountApiResponse.ApplicationStats source)
            {
                return new ApplicationStats
                {
                    Status = source.Status,
                    Count = source.Count
                };
            }
        }
    }
}
