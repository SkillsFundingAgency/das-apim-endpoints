using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.Application.Configuration;
using SFA.DAS.FindApprenticeshipTraining.Application.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Requests
{
    public class GetStandardsListRequest : IGetApiRequest
    {
        public string BaseUrl { get; set; }
        public string Keyword { get ; set ; }
        public List<Guid> RouteIds { get; set; }
        public string GetUrl => RouteIds!=null && RouteIds.Any() ? 
            $"{BaseUrl}api/courses/standards?keyword={Keyword}&routeIds=" + string.Join("&routeIds=",RouteIds) 
            : $"{BaseUrl}api/courses/standards?keyword={Keyword}";
    }
}
