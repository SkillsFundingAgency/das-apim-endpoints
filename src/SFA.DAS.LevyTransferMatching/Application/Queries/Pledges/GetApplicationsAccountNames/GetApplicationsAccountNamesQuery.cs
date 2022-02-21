using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationsAccountNames
{
    public class GetApplicationsAccountNamesQuery : IRequest<GetApplicationsAccountNamesQueryResult>
    {
        public int PledgeId { get; set; }
    }    
}
