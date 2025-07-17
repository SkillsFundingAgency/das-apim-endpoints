using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses
{
    public record GetApplicationsCountApiResponse
    {
        public List<Guid> ApplicationIds { get; set; } = [];
        public string Status { get; set; }
        public int Count { get; set; }
    }
}