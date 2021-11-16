using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetDeclined
{
    public class GetDeclinedQuery : IRequest<GetDeclinedResult>
    {
        public int ApplicationId { get; set; }
    }
}