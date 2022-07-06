using SFA.DAS.Approvals.InnerApi.Responses;
using System.Collections.Generic;


namespace SFA.DAS.Approvals.Application.ProviderUsers.Queries
{
    public class GetProviderUsersQueryResult
    {
        public IEnumerable<GetProviderUsersListItem> Users { get; set; }
    }
}