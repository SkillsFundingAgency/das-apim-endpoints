using MediatR;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;
public class GetLiveVacanciesQuery : IRequest<GetLiveVacanciesQueryResult>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}