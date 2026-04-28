using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EmployerAccounts;

public class GetUserByEmailRequest(string email) : IGetApiRequest
{
    public string Email { get; } = email;

    public string GetUrl => $"api/user?email={Email}";
}