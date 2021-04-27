using System.Collections.Generic;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetEmployerCourseProviderDemand
{
    public class GetEmployerCourseProviderDemandQueryResult
    {
        public GetStandardsListItem Course { get ; set ; }
        public LocationItem Location { get ; set ; }
        public IEnumerable<GetEmployerCourseProviderDemandResponse> EmployerCourseDemands { get ; set ; }
        public int Total { get ; set ; }
        public int TotalFiltered { get ; set ; }
        public GetProviderCourseInformation ProviderDetail { get ; set ; }
    }
}