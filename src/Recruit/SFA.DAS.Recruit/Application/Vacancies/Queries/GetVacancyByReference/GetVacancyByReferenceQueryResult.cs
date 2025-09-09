using SFA.DAS.Recruit.Domain.Vacancy;

namespace SFA.DAS.Recruit.Application.Vacancies.Queries.GetVacancyByReference;

public record GetVacancyByReferenceQueryResult(Vacancy Vacancy)
{
    public static readonly GetVacancyByReferenceQueryResult None = new((Vacancy)null);
}
