using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetGeoPoint
{
    public class GetGeoPointQueryResult
    {
        public GetGeoPointResponse GetPointResponse { get; }

        public GetGeoPointQueryResult(GetGeoPointResponse response)
        {
            GetPointResponse = response;
        }
    }
}
