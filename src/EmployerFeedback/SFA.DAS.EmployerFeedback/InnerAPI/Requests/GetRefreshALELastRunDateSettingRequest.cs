using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class GetRefreshALELastRunDateSettingRequest : IGetApiRequest
    {
        public string GetUrl => "api/settings/RefreshALELastRunDate";
    }
}
