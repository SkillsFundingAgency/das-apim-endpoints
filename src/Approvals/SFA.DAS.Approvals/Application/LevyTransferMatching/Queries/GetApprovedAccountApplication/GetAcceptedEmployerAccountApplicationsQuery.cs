using MediatR;

namespace SFA.DAS.Approvals.Application.LevyTransferMatching.Queries.GetApprovedAccountApplication
{
    public class GetAcceptedEmployerAccountApplicationsQuery : IRequest<GetAcceptedEmployerAccountApplicationsQueryResult>
    {
        public long EmployerAccountId { get; set; }
    }
}