using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;

namespace SFA.DAS.RecruitJobs.Api.Models.Responses;

public class GetAccountLegalEntitiesResponse
{
    public IEnumerable<GetAccountLegalEntityResponse> AccountLegalEntities { get; set; }
    
    public static GetAccountLegalEntitiesResponse From(IEnumerable<GetAccountLegalEntityResponseItem> source)
    {
        return new GetAccountLegalEntitiesResponse
        {
            AccountLegalEntities = source.Select(GetAccountLegalEntityResponse.From)
        };
    }
}

public class GetAccountLegalEntityResponseItem
{
    public List<Agreement> Agreements { get; set; }

    public string Address { get; set; }

    public string Name { get; set; }

    public string AccountLegalEntityPublicHashedId { get; set; }
    public long LegalEntityId { get; set; }
    public long AccountLegalEntityId { get; set; }
    public string DasAccountId { get; set; }
    public string AccountName { get; set; }
}

public class GetAccountLegalEntityResponse
{
    public bool HasLegalAgreement { get; set; }
    public string AccountLegalEntityPublicHashedId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public long AccountLegalEntityId { get; set; }
    public string DasAccountId { get; set; }
    public long LegalEntityId { get; set; }
    
    public static GetAccountLegalEntityResponse From(GetAccountLegalEntityResponseItem source)
    {
        return new GetAccountLegalEntityResponse
        {
            Address = source.Address,
            Name = source.Name,
            AccountLegalEntityPublicHashedId = source.AccountLegalEntityPublicHashedId,
            AccountLegalEntityId = source.AccountLegalEntityId,
            DasAccountId = source.DasAccountId,
            LegalEntityId = source.LegalEntityId,
            HasLegalAgreement = source.Agreements.Any(a =>
                a.Status == EmployerAgreementStatus.Signed)
        };
    }
}