using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts
{
    public class RefreshLegalEntitiesRequest : IPutApiRequest<RefreshLegalEntitiesRequestData>
    {
        public string PutUrl => "/jobs";

        public RefreshLegalEntitiesRequestData Data { get; set; }
    }

    public class RefreshLegalEntitiesRequestData
    {
        public JobType Type { get; set; }
        public IDictionary<string, object> Data { get; set; }
    }
}
