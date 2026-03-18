using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public class GetUserByIdamsIdRequest(string idamsId): IGetApiRequest
{
    public string GetUrl => $"api/user/by/idams/{idamsId}";
}