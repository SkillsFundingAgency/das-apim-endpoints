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
        public string GetUrl => BuildUrl();

        public List<int> Levels { get ; set ; }

        private string BuildUrl()
        {
            var url = $"{BaseUrl}api/courses/standards?keyword={Keyword}";

            if (RouteIds != null && RouteIds.Any())
            {
                url += "&routeIds=" + string.Join("&routeIds=", RouteIds);
            }

            if (Levels != null && Levels.Any())
            {
                url += "&levels=" + string.Join("&levels=", Levels);
            }

            return url;
        }
    }
}
