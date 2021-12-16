using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetGeocode
{
    public class GetGeoPointQuery : IRequest<GetGeoPointQueryResult>
    {
        public string Postcode { get; }

        public GetGeoPointQuery(string postcode)
        {
            Postcode = postcode;
        }
    }
}
