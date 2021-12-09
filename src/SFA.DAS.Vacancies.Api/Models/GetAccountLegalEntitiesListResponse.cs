using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.Vacancies.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer;
using SFA.DAS.Vacancies.Application.Providers.Queries.GetProviderAccountLegalEntities;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetAccountLegalEntitiesListResponse
    {
        public IReadOnlyList<GetAccountLegalEntitiesItem> AccountLegalEntities { get; set; }
        public static implicit operator GetAccountLegalEntitiesListResponse(
            GetLegalEntitiesForEmployerResult source)
        {
            if (source.LegalEntities == null)
            {
                return new GetAccountLegalEntitiesListResponse
                {
                    AccountLegalEntities = new List<GetAccountLegalEntitiesItem>()
                };
            }
            
            return new GetAccountLegalEntitiesListResponse
            {
                AccountLegalEntities = source.LegalEntities.Select(item => (GetAccountLegalEntitiesItem)item).ToList()
            };
        }

        public static implicit operator GetAccountLegalEntitiesListResponse(
            GetProviderAccountLegalEntitiesQueryResponse source)
        {
            if (source.ProviderAccountLegalEntities == null)
            {
                return new GetAccountLegalEntitiesListResponse
                {
                    AccountLegalEntities = new List<GetAccountLegalEntitiesItem>()
                };
            }
            
            return new GetAccountLegalEntitiesListResponse
            {
                AccountLegalEntities = source.ProviderAccountLegalEntities.Select(item => (GetAccountLegalEntitiesItem)item).ToList()
            };
        }
    }

    public class GetAccountLegalEntitiesItem
    {
        public string AccountLegalEntityName { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
        public string AccountPublicHashedId { get ; set ; }
        public string AccountName { get ; set ; }

        public static implicit operator GetAccountLegalEntitiesItem(GetEmployerAccountLegalEntityItem source)
        {
            return new GetAccountLegalEntitiesItem
            {
                AccountLegalEntityName = source.AccountLegalEntityName,
                AccountLegalEntityPublicHashedId = source.AccountLegalEntityPublicHashedId,
                AccountPublicHashedId = source.AccountPublicHashedId,
                AccountName = source.AccountName
            };
        }

        public static implicit operator GetAccountLegalEntitiesItem(GetProviderAccountLegalEntityItem source)
        {
            return new GetAccountLegalEntitiesItem
            {
                AccountLegalEntityName = source.AccountLegalEntityName,
                AccountLegalEntityPublicHashedId = source.AccountLegalEntityPublicHashedId,
                AccountPublicHashedId = source.AccountPublicHashedId,
                AccountName = source.AccountName
            };
        }
    }
}