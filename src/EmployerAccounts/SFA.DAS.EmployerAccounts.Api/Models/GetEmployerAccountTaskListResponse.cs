using SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerAccountTaskList;

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


