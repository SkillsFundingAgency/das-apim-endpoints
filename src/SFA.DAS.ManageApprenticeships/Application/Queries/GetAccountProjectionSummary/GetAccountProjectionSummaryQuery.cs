using MediatR;

namespace SFA.DAS.ManageApprenticeships.Application.Queries.GetAccountProjectionSummary
{
    public class GetAccountProjectionSummaryQuery : IRequest<GetAccountProjectionSummaryQueryResult>
    {
        public long AccountId { get; set; }
    }
}
