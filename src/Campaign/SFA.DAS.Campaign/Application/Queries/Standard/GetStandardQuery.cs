using MediatR;
using SFA.DAS.Campaign.Application.Queries.Standards;

namespace SFA.DAS.Campaign.Application.Queries.Standard
{
    public class GetStandardQuery : IRequest<GetStandardQueryResult>
    {
        public string StandardUId { get; set; }
    }
}
