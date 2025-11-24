using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.RecruitJobs.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitJobs.InnerApi.Requests.DelayedNotifications;
public record GetDelayedNotificationsByUserStatusRequest : IGetApiRequest
{
    public string GetUrl
    {
        get
        {
            const string baseUrl = "api/notifications/batch/by/userStatus";
            return QueryHelpers.AddQueryString(baseUrl, "status", nameof(UserStatus.Inactive));
        }
    }
}
