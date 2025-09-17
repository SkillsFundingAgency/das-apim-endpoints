using MediatR;

namespace SFA.DAS.Vacancies.Application.Vacancies.Queries.GetVacancy
{
    public class GetVacancyQuery : IRequest<GetVacancyQueryResult>
    {
        public string VacancyReference { get; set; }
    }
}