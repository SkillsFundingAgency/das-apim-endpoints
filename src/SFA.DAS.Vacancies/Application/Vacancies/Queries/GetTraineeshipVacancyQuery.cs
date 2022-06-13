using MediatR;

namespace SFA.DAS.Vacancies.Application.Vacancies.Queries
{
    public class GetTraineeshipVacancyQuery : IRequest<GetTraineeshipVacancyQueryResult>
    {
        public string VacancyReference { get; set; }
    }
}