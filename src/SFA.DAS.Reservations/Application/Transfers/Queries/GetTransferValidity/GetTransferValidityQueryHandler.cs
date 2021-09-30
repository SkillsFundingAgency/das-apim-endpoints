using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.Reservations.Application.Transfers.Queries.GetTransferValidity
{
    public class GetTransferValidityQueryHandler : IRequestHandler<GetTransferValidityQuery, GetTransferValidityQueryResult>
    {
        public GetTransferValidityQueryHandler()
        {
            
        }
        public Task<GetTransferValidityQueryResult> Handle(GetTransferValidityQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}