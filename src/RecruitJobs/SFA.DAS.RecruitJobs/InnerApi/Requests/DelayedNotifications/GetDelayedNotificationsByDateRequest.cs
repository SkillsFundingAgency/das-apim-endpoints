using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitJobs.InnerApi.Requests.DelayedNotifications;

public class GetDelayedNotificationsByDateRequest(DateTime dateTime): IGetApiRequest
{
    public string GetUrl {
        get
        {
            const string baseUrl = "api/notifications/batch/by/date";
            return QueryHelpers.AddQueryString(baseUrl, "dateTime", dateTime.ToString("s"));
        }
    }
}