using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetAccepted
{
    public class GetAcceptedQuery : IRequest<GetAcceptedResult>
    {
        public int ApplicationId { get; set; }
    }
}