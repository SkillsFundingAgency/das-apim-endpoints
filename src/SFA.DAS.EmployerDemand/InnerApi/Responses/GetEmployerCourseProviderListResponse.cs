using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerDemand.InnerApi.Responses
{
    public class GetEmployerCourseProviderListResponse
    {
        public IEnumerable<GetEmployerCourseProviderDemandResponse> EmployerCourseDemands { get; set; }
        public int TotalFiltered { get ; set ; }
        public int Total { get ; set ; }
    }

    public class GetEmployerCourseProviderDemandResponse
    {
        public Guid Id { get; set; }
        public int ApprenticesCount { get; set; }
        public Location Location { get; set; }
    }
}