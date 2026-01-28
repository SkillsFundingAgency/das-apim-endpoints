using SFA.DAS.ApprenticeCommitments.Apis.ApprenticeAccountsApi;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.Apprentices
{
    public class GetApprenticeByPersonalDetailQueryResponse
    {
        public List<Apprentice>? Apprentices { get; set; }
    }
}
