using System.Collections.Generic;

namespace SFA.DAS.Recruit.GraphQL.RecruitInner.Mappers;

public static class GqlVacancyStatusMapper
{
    public static readonly IReadOnlyDictionary<Domain.Vacancy.VacancyStatus, VacancyStatus>
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

    public static bool TryMapToGqlStatus(
        Domain.Vacancy.VacancyStatus status,
        out VacancyStatus gqlStatus)
        => Map.TryGetValue(status, out gqlStatus);
}