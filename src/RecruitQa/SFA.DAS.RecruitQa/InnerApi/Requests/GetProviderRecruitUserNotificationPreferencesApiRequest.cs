using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.RecruitQa.Domain;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class GetProviderRecruitUserNotificationPreferencesApiRequest(long ukprn, NotificationTypes? notificationType = null) : IGetAllApiRequest
{
    public string GetAllUrl
    {
        get
        {
            var url = $"api/user/by/ukprn/{ukprn}";
            if (notificationType is not null)
            {
                url = QueryHelpers.AddQueryString(url, new Dictionary<string, string> { { "notificationType", notificationType!.ToString()! } }!);
            }

            return url;
        }
    }
}