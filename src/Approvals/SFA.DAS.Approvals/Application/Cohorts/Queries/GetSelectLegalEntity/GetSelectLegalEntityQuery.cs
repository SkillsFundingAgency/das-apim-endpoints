using MediatR;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries.GetSelectLegalEntity
{
    public class GetSelectLegalEntityQuery(long accountId) : IRequest<GetSelectLegalEntityQueryResult>
    {
        public long AccountId { get; set; } = accountId;
    }
}
