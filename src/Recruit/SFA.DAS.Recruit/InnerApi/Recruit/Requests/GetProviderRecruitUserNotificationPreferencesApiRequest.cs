using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests;

public class GetProviderRecruitUserNotificationPreferencesApiRequest(long ukprn, NotificationTypes? notificationType = null) : IGetAllApiRequest
{
    public string GetAllUrl
    {
        get
        {
            var url = $"api/user/by/ukprn/{ukprn}";
            if (notificationType is not null)
            {
                url = QueryHelpers.AddQueryString(url, new Dictionary<string, string> { { "notificationType", notificationType.ToString() } });
            }

            return url;
        }
    }
}