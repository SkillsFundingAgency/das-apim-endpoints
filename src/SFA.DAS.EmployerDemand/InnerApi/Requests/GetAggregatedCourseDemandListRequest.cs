﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class GetAggregatedCourseDemandListRequest : IGetApiRequest
    {
        public string GetUrl => $"api/demand/aggregated/providers/{Ukprn}?courseId={CourseId}&lat={Lat}&lon={Lon}&radius={Radius}&sectors=" + string.Join("&sectors=", Sectors.Select(HttpUtility.UrlEncode));
        public int Ukprn { get; }
        public int? CourseId { get; }
        public double? Lat { get; }
        public double? Lon { get; }
        public int? Radius { get; }
        public List<string> Sectors { get; }

        public GetAggregatedCourseDemandListRequest(int ukprn, int? courseId = null, double? lat = null, double? lon = null, int? radius = null, List<string> sectors = null)
        {
            Ukprn = ukprn;
            CourseId = courseId;
            Lat = lat;
            Lon = lon;
            Radius = radius;
            Sectors = sectors;
        }
    }
}