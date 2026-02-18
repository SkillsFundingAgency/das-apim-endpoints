namespace SFA.DAS.Recruit.GraphQL.RecruitInner.Mappers;

public static class GqlVacancyStatusMapper
{
    public static bool TryMapToGqlStatus(Domain.Vacancy.VacancyStatus status, out VacancyStatus gqlStatus)
    {
        gqlStatus = default;

        return status switch
        {
            Domain.Vacancy.VacancyStatus.Draft => (gqlStatus = VacancyStatus.Draft) == VacancyStatus.Draft,
            Domain.Vacancy.VacancyStatus.Review => (gqlStatus = VacancyStatus.Review) == VacancyStatus.Review,
            _ => false
        };
    }
}
