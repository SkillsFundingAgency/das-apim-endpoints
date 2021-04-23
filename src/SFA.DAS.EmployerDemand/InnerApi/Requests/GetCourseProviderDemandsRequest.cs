using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Amqp.Serialization;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class GetCourseProviderDemandsRequest : IGetApiRequest
    {
        private readonly int _ukprn;
        private readonly int _courseId;
        private readonly double? _lat;
        private readonly double? _lon;
        private readonly int? _radius;
        private readonly List<string> _sectors;

        public GetCourseProviderDemandsRequest(int ukprn, int courseId,  double? lat = null, double? lon = null, int? radius = null, List<string> sectors = null)
        {
            _ukprn = ukprn;
            _courseId = courseId;
            _sectors = sectors;
            _lat = lat;
            _lon = lon;
            _radius = radius;
        }

        public string GetUrl => $"/api/Demand/providers/{_ukprn}/courses/{_courseId}?lat={_lat}&lon={_lon}&radius={_radius}&sectors=" + string.Join("&sectors=", _sectors.Select(HttpUtility.UrlEncode));
    }
}