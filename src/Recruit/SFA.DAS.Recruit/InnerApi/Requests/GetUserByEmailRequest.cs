using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Recruit.Enums;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public sealed record GetUserByEmailRequest(string Email, UserType UserType): IPostApiRequest
{
    public string PostUrl => "api/user/by/email";
    public object Data { get; set; } = new
    {
        Email,
        UserType
    };
}