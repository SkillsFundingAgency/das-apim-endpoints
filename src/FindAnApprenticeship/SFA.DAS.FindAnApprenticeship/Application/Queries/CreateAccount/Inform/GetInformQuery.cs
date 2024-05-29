using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.Inform
{
    public class GetInformQuery : IRequest<GetInformQueryResult>
    {
        public Guid CandidateId { get; set; }
    }

    public class GetInformQueryResult
    {
        public bool ShowAccountRecoveryBanner { get; set; }
    }

    public class GetInformQueryHandler : IRequestHandler<GetInformQuery, GetInformQueryResult>
    {
        public Task<GetInformQueryResult> Handle(GetInformQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
