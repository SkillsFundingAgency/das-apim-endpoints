using SFA.DAS.EarlyConnect.Application.Queries.GetLEPSDataWithUsers;

namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class GetLEPSDataListWithUsersResponse
    {
        public ICollection<GetLEPSDataWithUsersResponse>? LEPSData { get; set; }

        public static implicit operator GetLEPSDataListWithUsersResponse(GetLEPSDataWithUsersResult source)
        {
            return new GetLEPSDataListWithUsersResponse
            {
                LEPSData = source.LEPSData.Select(c => (GetLEPSDataWithUsersResponse)c).ToList()
            };
        }
    }
}