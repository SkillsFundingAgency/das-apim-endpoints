using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetActiveStandardsListRequest : IGetApiRequest
    {
        public List<Guid> RouteIds { get ; set ; }
        public List<int> Levels { get ; set ; }
        public string GetUrl => BuildUrl();

        private string BuildUrl()
        {
            var url = $"api/courses/standards?filter=Active";

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