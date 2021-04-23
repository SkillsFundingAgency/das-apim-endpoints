using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerDemand.InnerApi.Responses
{
    public class GetEmployerCourseProviderListResponse
    {
        public IEnumerable<GetEmployerCourseProviderDemandResponse> EmployerCourseDemands { get; set; }
        public int TotalFiltered { get ; set ; }
        public int Total { get ; set ; }
        public List<GetRoutesListItem> Sectors { get; set; }
    }

    public class GetEmployerCourseProviderDemandResponse
    {
        public Guid Id { get; set; }
        public int ApprenticesCount { get; set; }
        public Location Location { get; set; }
    }

    public class Location
    {
        public string Name { get; set; }
        public LocationPoint LocationPoint { get; set; }
    }

    public class LocationPoint
    {
        public List<double> GeoPoint { get; set; }
    }
}