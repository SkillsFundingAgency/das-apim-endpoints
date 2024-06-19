using MediatR;

namespace SFA.DAS.Approvals.Application.SelectProvider.Queries
{
    public class GetSelectProviderQuery : IRequest<GetSelectProviderQueryResult>
    {
        public long AccountLegalEntityId { get; set; }
    }
}