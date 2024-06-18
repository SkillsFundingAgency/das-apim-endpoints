using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Users.MigrateData
{
    public record MigrateDataQueryResult(GetLegacyApplicationsByEmailApiResponse Response)
    {
        public List<GetLegacyApplicationsByEmailApiResponse.Application> Applications { get; set; } = Response.Applications;
    }
}
