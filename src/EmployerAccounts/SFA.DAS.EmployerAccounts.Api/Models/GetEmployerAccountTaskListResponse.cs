using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerAccountTaskList;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerAccounts.Api.Models
{
    public class GetEmployerAccountTaskListResponse
    {
        public bool HasProviders { get; set; }

        public bool HasPermissions { get; set; }
        
        public static implicit operator GetEmployerAccountTaskListResponse(GetEmployerAccountTaskListQueryResult source)
        {
            return new GetEmployerAccountTaskListResponse
            {
                HasPermissions = source.HasPermissions,
                HasProviders = source.HasProviders
            };
        }
    }
}


