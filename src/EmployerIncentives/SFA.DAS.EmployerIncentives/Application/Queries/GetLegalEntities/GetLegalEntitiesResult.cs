using System.Collections.Generic;
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntities
{
    public class GetLegalEntitiesResult
    {
        public IEnumerable<AccountLegalEntity> AccountLegalEntities { get ; set ; }
    }
}