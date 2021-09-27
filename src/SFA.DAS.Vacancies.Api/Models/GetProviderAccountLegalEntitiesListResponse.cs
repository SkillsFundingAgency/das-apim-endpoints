using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Vacancies.Application.Providers.Queries.GetProviderAccountLegalEntities;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetProviderAccountLegalEntitiesListResponse
    {
        public List<GetProviderAccountLegalEntitiesListItem> ProviderAccountLegalEntities { get ; set ; }

        public static implicit operator GetProviderAccountLegalEntitiesListResponse(
            GetProviderAccountLegalEntitiesQueryResponse source)
        {
            if (source.ProviderAccountLegalEntities == null)
            {
                return new GetProviderAccountLegalEntitiesListResponse
                {
                    ProviderAccountLegalEntities = new List<GetProviderAccountLegalEntitiesListItem>()
                };
            }
            
            return new GetProviderAccountLegalEntitiesListResponse
            {
                ProviderAccountLegalEntities = source.ProviderAccountLegalEntities.Select(c=>(GetProviderAccountLegalEntitiesListItem)c).ToList()
            };
        }
    }

    public class GetProviderAccountLegalEntitiesListItem
    {
        public string AccountLegalEntityPublicHashedId { get ; set ; }
        public string AccountPublicHashedId { get ; set ; }
        public string AccountLegalEntityName { get ; set ; }
        public string AccountName { get ; set ; }

        public static implicit operator GetProviderAccountLegalEntitiesListItem(GetProviderAccountLegalEntityItem source)
        {
            return new GetProviderAccountLegalEntitiesListItem
            {
                AccountName = source.AccountName,
                AccountLegalEntityName = source.AccountLegalEntityName,
                AccountPublicHashedId = source.AccountPublicHashedId,
                AccountLegalEntityPublicHashedId = source.AccountLegalEntityPublicHashedId
            };
        }
    }
}