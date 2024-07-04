using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Models;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Users.MigrateData
{
    public record MigrateDataQueryResult(
        GetLegacyApplicationsByEmailApiResponse Response,
        GetLegacyUserByEmailApiResponse UserDetails,
        GetCandidateApiResponse ExistingUser)
    {
        public List<GetLegacyApplicationsByEmailApiResponse.Application> Applications { get; set; } = Response.Applications;

        public RegistrationDetails LegacyUserDetail { get; set; } = UserDetails.RegistrationDetails;
        public GetCandidateApiResponse CandidateDetail { get; set; } = ExistingUser;
    }
}
