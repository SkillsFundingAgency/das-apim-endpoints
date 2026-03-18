using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetRegisterDemand
{
    public class GetRegisterDemandResult
    {
        public GetStandardsListItem Course { get; set; }
        public LocationItem Location { get ; set ; }
    }
}