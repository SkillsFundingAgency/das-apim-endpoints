using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetSelectAccount
{
    public class GetSelectAccountQuery : IRequest<GetSelectAccountQueryResult>
    {
        public string UserId { get; set; }
    }
}