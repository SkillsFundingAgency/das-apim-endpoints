using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.Approvals.Application.Authorization.Queries;

public class GetCohortAccessQueryHandler : IRequestHandler<GetCohortAccessQuery, bool>
{
    public Task<bool> Handle(GetCohortAccessQuery request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}