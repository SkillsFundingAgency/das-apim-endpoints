using System.Collections.Generic;

namespace SFA.DAS.Vacancies.InnerApi.Responses
{
    public class GetProviderAccountLegalEntitiesResponse
    {
        public IEnumerable<GetProviderAccountLegalEntityItem> AccountProviderLegalEntities { get; set; }
    }

    public class GetProviderAccountLegalEntityItem
    {
        public long AccountId { get; set; }
        public string AccountPublicHashedId { get; set; }
        public string AccountName { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
        public string AccountLegalEntityName { get; set; }
        public long AccountProviderId { get; set; }
    }
}