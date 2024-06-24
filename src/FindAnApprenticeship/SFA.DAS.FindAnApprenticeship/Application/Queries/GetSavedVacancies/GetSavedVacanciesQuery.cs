using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetSavedVacancies
{
    public class GetSavedVacanciesQuery : IRequest<GetSavedVacanciesQueryResult>
    {
        public Guid CandidateId { get; set; }
    }

    public class GetSavedVacanciesQueryResult
    {

    }

    public class GetSavedVacanciesQueryHandler : IRequestHandler<GetSavedVacanciesQuery, GetSavedVacanciesQueryResult>
    {
        public async Task<GetSavedVacanciesQueryResult> Handle(GetSavedVacanciesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
