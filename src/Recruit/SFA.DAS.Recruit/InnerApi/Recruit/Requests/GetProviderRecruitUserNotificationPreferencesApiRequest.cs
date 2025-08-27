using SFA.DAS.Recruit.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests;

public class GetProviderRecruitUserNotificationPreferencesApiRequest(int ukprn, NotificationTypes? notificationType = null) : IGetAllApiRequest
{
    public string GetAllUrl => $"api/user/by/ukprn/{ukprn}?notificationType={notificationType}";
}