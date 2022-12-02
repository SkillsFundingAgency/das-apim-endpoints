#nullable enable
using MediatR;

namespace SFA.DAS.Campaign.Application.Queries.Standards
{
    public class GetStandardsQuery : IRequest<GetStandardsQueryResult>
    {
        public string? Sector { get; set; }
    }
}