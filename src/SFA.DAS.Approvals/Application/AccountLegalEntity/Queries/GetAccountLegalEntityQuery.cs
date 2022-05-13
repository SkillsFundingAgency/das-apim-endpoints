using MediatR;

namespace SFA.DAS.Approvals.Application.AccountLegalEntity
{
    public class GetAccountLegalEntityQuery : IRequest<GetAccountLegalEntityQueryResult>
    {
        public long AccountLegalEntityId { get; set; }
    }
}
