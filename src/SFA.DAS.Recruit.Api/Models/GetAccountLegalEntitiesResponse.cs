using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Recruit.Application.Queries.GetAccountLegalEntities;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Recruit.Api.Models
{
    public class GetAccountLegalEntitiesResponse
    {
        public static implicit operator GetAccountLegalEntitiesResponse(GetAccountLegalEntitiesQueryResult source)
        {
            return new GetAccountLegalEntitiesResponse
            {
                AccountLegalEntities = source.AccountLegalEntities.Select(c=>(GetAccountLegalEntityResponse)c).ToList()
            };
        }
        public List<GetAccountLegalEntityResponse> AccountLegalEntities { get; set; }
    }

    public class GetAccountLegalEntityResponse
    {
        public bool HasLegalAgreement { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public static implicit operator GetAccountLegalEntityResponse(GetAccountLegalEntityResponseItem source)
        {
            return new GetAccountLegalEntityResponse
            {
                Address = source.Address,
                Name = source.Name,
                AccountLegalEntityPublicHashedId = source.AccountLegalEntityPublicHashedId,
                HasLegalAgreement = source.Agreements.Any(a =>
                a.Status == EmployerAgreementStatus.Signed)
            };
        }
    }
}