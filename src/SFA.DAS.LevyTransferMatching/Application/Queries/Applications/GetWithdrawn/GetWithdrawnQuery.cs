using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetWithdrawn
{
    public class GetWithdrawnQuery : IRequest<GetWithdrawnQueryResult>
    {
        public int ApplicationId { get; set; }
    }
}