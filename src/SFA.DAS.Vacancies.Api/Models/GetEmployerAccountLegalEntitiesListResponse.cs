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
        public string Code { get; set; }
        public string DasAccountId { get; set; }
        public DateTime? DateOfInception { get; set; }
        public string Address { get; set; }
        public long LegalEntityId { get; set; }
        public string Name { get; set; }
        public string PublicSectorDataSource { get; set; }
        public string Sector { get; set; }
        public string Source { get; set; }
        public short SourceNumeric { get; set; }
        public string Status { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }

        public static implicit operator GetEmployerAccountLegalEntitiesItem(
            GetEmployerAccountLegalEntityItem source)
        {
            return new GetEmployerAccountLegalEntitiesItem
            {
                Code = source.Code,
                DasAccountId = source.DasAccountId,
                DateOfInception = source.DateOfInception,
                Address = source.Address,
                LegalEntityId = source.LegalEntityId,
                Name = source.Name,
                PublicSectorDataSource = source.PublicSectorDataSource,
                Sector = source.Sector,
                Source = source.Source,
                SourceNumeric = source.SourceNumeric,
                Status = source.Status,
                AccountLegalEntityId = source.AccountLegalEntityId,
                AccountLegalEntityPublicHashedId = source.AccountLegalEntityPublicHashedId
            };
        }
    }
}