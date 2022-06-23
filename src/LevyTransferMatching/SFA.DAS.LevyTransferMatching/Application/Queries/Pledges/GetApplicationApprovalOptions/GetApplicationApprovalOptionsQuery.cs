using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationApprovalOptions
{
    public class GetApplicationApprovalOptionsQuery : IRequest<GetApplicationApprovalOptionsQueryResult>
    {
        public int PledgeId { get; set; }
        public int ApplicationId { get; set; }
    }
}
