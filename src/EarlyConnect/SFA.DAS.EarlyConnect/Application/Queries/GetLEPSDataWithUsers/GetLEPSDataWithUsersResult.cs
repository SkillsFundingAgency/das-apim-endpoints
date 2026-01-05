using SFA.DAS.EarlyConnect.InnerApi.Responses;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetLEPSDataWithUsers
{
    public class GetLEPSDataWithUsersResult
    {
        public ICollection<LEPSData>? LEPSData { get; set; }
    }
}