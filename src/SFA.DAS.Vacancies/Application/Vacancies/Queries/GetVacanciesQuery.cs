using MediatR;

namespace SFA.DAS.Vacancies.Application.Vacancies.Queries
{
    public class GetVacanciesQuery: IRequest<GetVacanciesQueryResult>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
