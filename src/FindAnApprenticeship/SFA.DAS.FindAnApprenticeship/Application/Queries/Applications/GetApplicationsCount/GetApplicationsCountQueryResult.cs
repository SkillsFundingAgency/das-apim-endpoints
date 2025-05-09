using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplicationsCount
{
    public record GetApplicationsCountQueryResult
    {
        public List<Guid> ApplicationIds { get; set; } = [];
        public string Status { get; set; }
        public int Count { get; set; }

        public static implicit operator GetApplicationsCountQueryResult(GetApplicationsCountApiResponse source)
        {
            return new GetApplicationsCountQueryResult
            {
                Status = source.Status,
                Count = source.Count,
                ApplicationIds = source.ApplicationIds
            };
        }
    }
}
