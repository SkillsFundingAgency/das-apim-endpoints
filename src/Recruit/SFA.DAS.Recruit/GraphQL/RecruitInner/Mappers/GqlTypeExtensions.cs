using System;

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
}