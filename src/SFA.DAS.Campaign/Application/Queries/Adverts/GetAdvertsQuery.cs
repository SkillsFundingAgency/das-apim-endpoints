using MediatR;

namespace SFA.DAS.Campaign.Application.Queries.Adverts
{
    public class GetAdvertsQuery : IRequest<GetAdvertsQueryResult>
    {
        public string Route { get ; set ; }
        public string Postcode { get ; set ; }
        public uint Distance { get; set; }
    }
}