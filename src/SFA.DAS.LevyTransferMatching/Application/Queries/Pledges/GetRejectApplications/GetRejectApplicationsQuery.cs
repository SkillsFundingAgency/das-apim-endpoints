using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetRejectApplications
{
    public class GetRejectApplicationsQuery : IRequest<GetRejectApplicationsQueryResult>
    {
        public int PledgeId { get; set; }
    }    
}
