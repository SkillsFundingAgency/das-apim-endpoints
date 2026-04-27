using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public class GetUserByDfeUserIdRequest(string dfeUserId): IGetApiRequest
{
    public string GetUrl => $"api/user/by/dfeuserid/{dfeUserId}";
}