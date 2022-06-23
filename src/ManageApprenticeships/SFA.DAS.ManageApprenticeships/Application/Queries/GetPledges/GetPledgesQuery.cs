using MediatR;

namespace SFA.DAS.ManageApprenticeships.Application.Queries.GetPledges
{
    public class GetPledgesQuery : IRequest<GetPledgesQueryResult>
    {
        public long AccountId { get; set; }
    }
}