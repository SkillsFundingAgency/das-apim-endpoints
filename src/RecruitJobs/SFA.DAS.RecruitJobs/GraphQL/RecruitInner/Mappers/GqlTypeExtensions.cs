using SFA.DAS.RecruitJobs.Domain.Vacancy;

namespace SFA.DAS.RecruitJobs.GraphQL.RecruitInner.Mappers;

public static class GqlTypeExtensions
{
    public static VacancyStatus FromQueryType(this VacancyStatus status)
    {
        return status switch
        {
            VacancyStatus.Draft => VacancyStatus.Draft,
            VacancyStatus.Review => VacancyStatus.Review,
            VacancyStatus.Rejected => VacancyStatus.Rejected,
            VacancyStatus.Submitted => VacancyStatus.Submitted,
            VacancyStatus.Referred => VacancyStatus.Referred,
            VacancyStatus.Live => VacancyStatus.Live,
            VacancyStatus.Closed => VacancyStatus.Closed,
            VacancyStatus.Approved => VacancyStatus.Approved,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }

    public static OwnerType? FromQueryType(this OwnerType? ownerType)
    {
        if (ownerType is null)
        {
            return null;
        }
        
        return ownerType switch
        {
            OwnerType.Employer => OwnerType.Employer,
            OwnerType.Provider => OwnerType.Provider,
            OwnerType.External => OwnerType.External,
            OwnerType.Unknown => OwnerType.Unknown,
            _ => throw new ArgumentOutOfRangeException(nameof(ownerType), ownerType, null)
        };
    }
}