using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Responses;


namespace SFA.DAS.Approvals.Application.ProviderUsers.Queries
{
    public class GetProviderUsersQueryResult
    {
        public IEnumerable<GetProviderUsersListItem> Users { get; set; }
    }
}