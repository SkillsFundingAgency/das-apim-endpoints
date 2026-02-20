using System;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.GraphQL.RecruitInner.Mappers;

public static class GqlTypeExtensions
{
    public static Domain.Vacancy.VacancyStatus FromQueryType(this VacancyStatus status)
    {
        return status switch
        {
            VacancyStatus.Draft => Domain.Vacancy.VacancyStatus.Draft,
            VacancyStatus.Review => Domain.Vacancy.VacancyStatus.Review,
            VacancyStatus.Rejected => Domain.Vacancy.VacancyStatus.Rejected,
            VacancyStatus.Submitted => Domain.Vacancy.VacancyStatus.Submitted,
            VacancyStatus.Referred => Domain.Vacancy.VacancyStatus.Referred,
            VacancyStatus.Live => Domain.Vacancy.VacancyStatus.Live,
            VacancyStatus.Closed => Domain.Vacancy.VacancyStatus.Closed,
            VacancyStatus.Approved => Domain.Vacancy.VacancyStatus.Approved,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }

    public static Domain.Vacancy.OwnerType? FromQueryType(this OwnerType? ownerType)
    {
        if (ownerType is null)
        {
            return null;
        }
        
        return ownerType switch
        {
            OwnerType.Employer => Domain.Vacancy.OwnerType.Employer,
            OwnerType.Provider => Domain.Vacancy.OwnerType.Provider,
            OwnerType.External => Domain.Vacancy.OwnerType.External,
            OwnerType.Unknown => Domain.Vacancy.OwnerType.Unknown,
            _ => throw new ArgumentOutOfRangeException(nameof(ownerType), ownerType, null)
        };
    }
    
    public static Domain.Vacancy.SourceOrigin? FromQueryType(this SourceOrigin? sourceOrigin)
    {
        return sourceOrigin switch
        {
            SourceOrigin.Api => Domain.Vacancy.SourceOrigin.Api,
            SourceOrigin.EmployerWeb => Domain.Vacancy.SourceOrigin.EmployerWeb,
            SourceOrigin.ProviderWeb => Domain.Vacancy.SourceOrigin.ProviderWeb,
            SourceOrigin.WebComplaint => Domain.Vacancy.SourceOrigin.WebComplaint,
            null => null,
            _ => throw new ArgumentOutOfRangeException(nameof(sourceOrigin), sourceOrigin, null)
        };
    }
    
    public static SharedOuterApi.Domain.ApprenticeshipTypes? FromQueryType(this ApprenticeshipTypes? sourceOrigin)
    {
        return sourceOrigin switch
        {
            ApprenticeshipTypes.Standard => SharedOuterApi.Domain.ApprenticeshipTypes.Standard,
            ApprenticeshipTypes.Foundation => SharedOuterApi.Domain.ApprenticeshipTypes.Foundation,
            null => null,
            _ => throw new ArgumentOutOfRangeException(nameof(sourceOrigin), sourceOrigin, null)
        };
    }
    
    public static Domain.Vacancy.ApplicationMethod? FromQueryType(this ApplicationMethod? sourceOrigin)
    {
        return sourceOrigin switch
        {
            ApplicationMethod.ThroughFindAnApprenticeship => Domain.Vacancy.ApplicationMethod.ThroughFindAnApprenticeship, 
            ApplicationMethod.ThroughExternalApplicationSite => Domain.Vacancy.ApplicationMethod.ThroughExternalApplicationSite, 
            ApplicationMethod.ThroughFindATraineeship => Domain.Vacancy.ApplicationMethod.ThroughFindATraineeship, 
            ApplicationMethod.Unspecified => Domain.Vacancy.ApplicationMethod.Unspecified, 
            null => null,
            _ => throw new ArgumentOutOfRangeException(nameof(sourceOrigin), sourceOrigin, null)
        };
    }

    public static bool TryMapToGqlStatus(
        Domain.Vacancy.VacancyStatus status,
        out VacancyStatus gqlStatus)
        => Map.TryGetValue(status, out gqlStatus);

    private static readonly IReadOnlyDictionary<Domain.Vacancy.VacancyStatus, VacancyStatus>
        Map = new Dictionary<Domain.Vacancy.VacancyStatus, VacancyStatus>
        {
            [Domain.Vacancy.VacancyStatus.Draft] = VacancyStatus.Draft,
            [Domain.Vacancy.VacancyStatus.Review] = VacancyStatus.Review,
            [Domain.Vacancy.VacancyStatus.Submitted] = VacancyStatus.Submitted,
            [Domain.Vacancy.VacancyStatus.Approved] = VacancyStatus.Approved,
            [Domain.Vacancy.VacancyStatus.Closed] = VacancyStatus.Closed,
            [Domain.Vacancy.VacancyStatus.Live] = VacancyStatus.Live,
            [Domain.Vacancy.VacancyStatus.Referred] = VacancyStatus.Referred,
            [Domain.Vacancy.VacancyStatus.Rejected] = VacancyStatus.Rejected,
        };
}