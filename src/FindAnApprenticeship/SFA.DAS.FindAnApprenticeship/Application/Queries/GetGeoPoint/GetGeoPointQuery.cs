using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetGeoPoint
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
