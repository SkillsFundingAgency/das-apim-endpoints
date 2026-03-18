using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class GetRefreshALELastRunDateSettingRequest : IGetApiRequest
    {
        public string GetUrl => "api/settings/RefreshALELastRunDate";
    }
}
