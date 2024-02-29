using MediatR;

namespace SFA.DAS.Campaign.Application.Queries.Standard
{
    public class GetStandardQuery : IRequest<GetStandardQueryResult>
    {
        public string StandardUId { get; set; }
    }
}
