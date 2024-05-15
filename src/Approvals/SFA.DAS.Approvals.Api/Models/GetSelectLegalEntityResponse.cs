using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetSelectLegalEntity;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.Models
{
    public class GetSelectLegalEntityResponse
    {
        public static implicit operator GetSelectLegalEntityResponse(GetSelectLegalEntityQueryResult source)
        {
            return new GetSelectLegalEntityResponse
            {
                LegalEntities = source.LegalEntities
                    .Select(c => (GetLegalEntityResponse)c).ToList()
            };
        }
        public List<GetLegalEntityResponse> LegalEntities { get; set; }
    }

    public class GetLegalEntityResponse
    {
        public bool HasLegalAgreement { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string HashedAccountId { get; set; }
        public long LegalEntityId { get; set; }
        public static implicit operator GetLegalEntityResponse(GetLegalEntitiesForAccountResponseItem source)
        {
            return new GetLegalEntityResponse
            {
                Address = source.Address,
                Name = source.Name,
                AccountLegalEntityPublicHashedId = source.AccountLegalEntityPublicHashedId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                HashedAccountId = source.HashedAccountId,
                LegalEntityId = source.LegalEntityId,
                HasLegalAgreement = source.Agreements.Any(a =>
                a.Status == EmployerAgreementStatus.Signed)
            };
        }
    }
}