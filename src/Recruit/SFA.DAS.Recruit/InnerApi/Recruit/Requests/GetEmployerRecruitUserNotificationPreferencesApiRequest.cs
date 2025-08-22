using SFA.DAS.Recruit.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests;

public class GetEmployerRecruitUserNotificationPreferencesApiRequest(long accountId, NotificationTypes? notificationType = null) : IGetAllApiRequest
{
    public string GetAllUrl => $"api/user/by/employeraccountid/{accountId}?notificationType={notificationType}";
}