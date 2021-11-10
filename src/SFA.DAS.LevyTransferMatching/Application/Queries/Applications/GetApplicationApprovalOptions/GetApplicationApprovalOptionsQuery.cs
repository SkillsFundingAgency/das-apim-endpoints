using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplicationApprovalOptions
{
    public class GetApplicationApprovalOptionsQuery : IRequest<GetApplicationApprovalOptionsResult>
    {
        public int PledgeId { get; set; }
        public int ApplicationId { get; set; }
    }
}