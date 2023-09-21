using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerAccountTaskList;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerAccounts.Api.Models
{
    public class GetEmployerAccountTaskListResponse
    {
        public IEnumerable<EmployerAccountLegalEntityPermissionItem> EmployerAccountLegalEntityPermissions { get; set; }

        public class EmployerAccountLegalEntityPermissionItem
        {
            public string AccountLegalEntityPublicHashedId { get; set; }
            public string Name { get; set; }
            public string AccountHashedId { get ; set ; }
            
            public static implicit operator EmployerAccountLegalEntityPermissionItem(AccountLegalEntityItem source)
            {
                return new EmployerAccountLegalEntityPermissionItem()
                {
                    AccountHashedId = source.AccountHashedId,
                    AccountLegalEntityPublicHashedId = source.AccountLegalEntityPublicHashedId,
                    Name = source.Name
                };
            }
        }
        
        public static implicit operator GetEmployerAccountTaskListResponse(GetEmployerAccountTaskListQueryResult source)
        {
            return new GetEmployerAccountTaskListResponse
            {
                EmployerAccountLegalEntityPermissions = source.EmployerAccountLegalEntityPermissions?
                    .Where(x => x != null)
                    .Select(x => (EmployerAccountLegalEntityPermissionItem)x)
            };
        }
    }
}


