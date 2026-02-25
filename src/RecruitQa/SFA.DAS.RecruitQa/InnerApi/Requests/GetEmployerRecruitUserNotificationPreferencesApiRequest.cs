using SFA.DAS.RecruitQa.Domain;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class GetEmployerRecruitUserNotificationPreferencesApiRequest(long accountId, NotificationTypes? notificationType = null) : IGetAllApiRequest
{
    public string GetAllUrl => $"api/user/by/employeraccountid/{accountId}?notificationType={notificationType}";
}