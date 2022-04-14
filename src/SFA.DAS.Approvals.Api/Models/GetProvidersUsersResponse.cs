using SFA.DAS.Approvals.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.Api.Models
{
    public class GetProvidersUsersResponse
    {
        public IEnumerable<GetProviderUsersListItem> Users { get; set; }

    }
}