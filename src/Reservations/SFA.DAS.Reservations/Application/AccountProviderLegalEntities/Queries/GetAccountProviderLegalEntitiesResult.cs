using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ProviderRelationships;

namespace SFA.DAS.Reservations.Application.AccountProviderLegalEntities.Queries
{
    public class GetAccountProviderLegalEntitiesResult
    {
        public GetProviderAccountLegalEntitiesResponse AccountProviderLegalEntities { get; set; }
    }
}
