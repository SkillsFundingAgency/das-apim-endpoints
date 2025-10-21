using SFA.DAS.Recruit.Domain.Vacancy;

namespace SFA.DAS.Recruit.Application.Vacancies.Queries.GetVacancyById;

public record GetVacancyByIdQueryResult(Vacancy Vacancy)
{
    public static readonly GetVacancyByIdQueryResult None = new ((Vacancy)null);
}