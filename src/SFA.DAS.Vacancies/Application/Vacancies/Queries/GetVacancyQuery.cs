using MediatR;

namespace SFA.DAS.Vacancies.Application.Vacancies.Queries
{
    public class GetVacancyQuery : IRequest<GetVacancyQueryResult>
    {
        public string VacancyReference { get; set; }
    }
}