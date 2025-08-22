using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests;

public class GetEmployerRecruitUserNotificationPreferencesApiRequest(long accountId) : IGetAllApiRequest
{
    public string GetAllUrl => $"api/user/by/employeraccountid/{accountId}";
}