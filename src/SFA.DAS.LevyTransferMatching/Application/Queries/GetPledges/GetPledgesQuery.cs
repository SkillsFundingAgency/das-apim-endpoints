using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetAllPledges
{
    public class GetPledgesQuery : IRequest<GetPledgesResult>
    {
        public string EncodedId { get; set; }
    }
}
