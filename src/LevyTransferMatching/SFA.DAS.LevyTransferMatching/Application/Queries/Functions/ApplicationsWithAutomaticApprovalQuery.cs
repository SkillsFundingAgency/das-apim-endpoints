using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Functions
{
    public class ApplicationsWithAutomaticApprovalQuery : IRequest<ApplicationsWithAutomaticApprovalQueryResult>
    {
        public int? PledgeId { get; set; }
    }

}
