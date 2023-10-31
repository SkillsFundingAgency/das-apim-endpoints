using MediatR;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;
public class GetLiveVacanciesQueryHandler : IRequestHandler<GetLiveVacanciesQuery, GetLiveVacanciesQueryResult>
{
    Task<GetLiveVacanciesQueryResult> IRequestHandler<GetLiveVacanciesQuery, GetLiveVacanciesQueryResult>.Handle(GetLiveVacanciesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
