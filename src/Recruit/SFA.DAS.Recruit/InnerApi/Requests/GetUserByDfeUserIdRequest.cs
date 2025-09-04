using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public class GetUserByDfeUserIdRequest(string dfeUserId): IGetApiRequest
{
    public string GetUrl => $"api/user/by/dfeuserid/{dfeUserId}";
}