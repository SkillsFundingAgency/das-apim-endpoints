using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetEmployerCourseProviderDemand
{
    public class GetEmployerCourseProviderDemandQueryResult
    {
        public GetStandardsListItem Course { get ; set ; }
        public LocationItem Location { get ; set ; }
        public object EmployerCourseDemands { get ; set ; }
    }
}