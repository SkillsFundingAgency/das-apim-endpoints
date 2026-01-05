using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class GetLEPSDataWithUsersRequest : IGetApiRequest
    {
        public string GetUrl => "api/leps-data";
    }
}