using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Vacancies.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetEmployerAccountLegalEntitiesListResponse
    {
        public IReadOnlyList<GetEmployerAccountLegalEntitiesItem> EmployerAccountLegalEntities { get; set; }

        public static implicit operator GetEmployerAccountLegalEntitiesListResponse(
            GetLegalEntitiesForEmployerResult source)
        {
            return new GetEmployerAccountLegalEntitiesListResponse
            {
                EmployerAccountLegalEntities = source.LegalEntities.Select(item => (GetEmployerAccountLegalEntitiesItem)item).ToList()
            };
        }
    }

    public class GetEmployerAccountLegalEntitiesItem
    {
        public string Address { get; set; }
        public string Name { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }

        public static implicit operator GetEmployerAccountLegalEntitiesItem(
            GetEmployerAccountLegalEntityItem source)
        {
            return new GetEmployerAccountLegalEntitiesItem
            {
                Address = source.Address,
                Name = source.Name,
                AccountLegalEntityPublicHashedId = source.AccountLegalEntityPublicHashedId
            };
        }
    }
}