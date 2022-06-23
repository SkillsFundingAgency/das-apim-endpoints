using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Standards
{
    public class GetStandardsQuery : IRequest<GetStandardsQueryResult>
    {
        public string StandardId { get; set; }
    }
}