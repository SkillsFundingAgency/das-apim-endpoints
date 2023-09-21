using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerAccountTaskList
{
    public class GetEmployerAccountTaskListQueryResult
    {
        public IEnumerable<AccountLegalEntityItem> EmployerAccountLegalEntityPermissions { get; set; }
    }
}