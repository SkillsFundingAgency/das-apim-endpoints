using System.Collections.Generic;
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.InnerApi.Responses
{
    public class GetAccountLegalEntitiesResponse
    {
        public IEnumerable<AccountLegalEntity> AccountLegalEntities { get; set; }
    }
}