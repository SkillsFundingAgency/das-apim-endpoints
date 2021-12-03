using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Application.Queries.GetGeocode
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
