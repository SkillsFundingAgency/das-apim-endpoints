using MediatR;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries
{
    public class GetLiveVacancyQuery : IRequest<GetLiveVacancyQueryResult>
    {
        public long VacancyReference { get; set; }
    }
}
