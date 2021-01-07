using SFA.DAS.EmployerIncentives.InnerApi.Responses.Accounts;
using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts
{
    public class RefreshLegalEntitiesRequestData
    {
        public IEnumerable<AccountLegalEntity> AccountLegalEntities { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }      
    }
}
