using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
public class SearchOrganisationResponse
{
    public IEnumerable<Organisation> SearchResults { get; set; }

    public int TotalCount { get; set; }
}

public class Organisation : BaseEntity
{
    public Guid Id { get; set; }
    public ProviderType ProviderType { get; set; }
    public OrganisationType OrganisationType { get; set; }
    public long UKPRN { get; set; }
    public string LegalName { get; set; }
    public string TradingName { get; set; }
    public OrganisationStatus OrganisationStatus { get; set; }
    public DateTime StatusDate { get; set; }
    public OrganisationData OrganisationData { get; set; }
}

public class BaseEntity
{
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string Status { get; set; }
}

public class ProviderType : BaseEntity
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
}

public class OrganisationType : BaseEntity
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
}

public class OrganisationStatus : BaseEntity
{
    public int Id { get; set; }
}

public class OrganisationData
{
    public string CompanyNumber { get; set; }
    public string CharityNumber { get; set; }
    public RemovedReason RemovedReason { get; set; }
    public bool ParentCompanyGuarantee { get; set; }
    public bool FinancialTrackRecord { get; set; }
    public bool NonLevyContract { get; set; }
    public DateTime? StartDate { get; set; }
    public bool? SourceIsUKRLP { get; set; }

    public DateTime? ApplicationDeterminedDate { get; set; }
}

public class RemovedReason : BaseEntity
{
    public int Id { get; set; }
    public string Reason { get; set; }
    public string Description { get; set; }
}

public class ProviderStatusType : BaseEntity
{
    public int Id { get; set; }
    public string Type { get; set; }

}