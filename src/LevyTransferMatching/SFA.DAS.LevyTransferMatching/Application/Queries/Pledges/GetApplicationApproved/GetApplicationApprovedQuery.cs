using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationApproved
{
    public class GetApplicationApprovedQuery : IRequest<GetApplicationApprovedQueryResult>
    {
        public int PledgeId { get; set; }
        public int ApplicationId { get; set; }
    }
}
