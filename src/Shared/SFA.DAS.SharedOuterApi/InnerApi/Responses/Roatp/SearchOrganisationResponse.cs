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

    public const int MainProvider = 1;
    public const int EmployerProvider = 2;
    public const int SupportingProvider = 3;
}

public class OrganisationType : BaseEntity
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }

    public const int Unassigned = 0;
}

public class OrganisationStatus : BaseEntity
{
    public int Id { get; set; }

    public const int Removed = 0;
    public const int Active = 1;
    public const int ActiveNotTakingOnApprentices = 2;
    public const int Onboarding = 3;
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

    public const int Breach = 1;
    public const int ChangeOfTradingStatus = 2;
    public const int HighRiskPolicy = 3;
    public const int InadequateFinancialHealth = 4;
    public const int InadequateOfstedGrade = 5;
    public const int InternalError = 6;
    public const int Merger = 7;
    public const int MinimumStandardsNotMet = 8;
    public const int NonDirectDeliveryInTwelveMonthPeriod = 9;
    public const int ProviderError = 10;
    public const int ProviderRequest = 11;
    public const int Other = 12;
    public const int NoDeliveryInA6MonthPeriod = 13;
    public const int TwoInsufficientProgressOfstedMonitoring = 14;
    public const int FailedAparApplication = 15;
    public const int DidNotReApplyWhenRequested = 16;
    public const int GapInProvisionCondition5Breach = 17;
}