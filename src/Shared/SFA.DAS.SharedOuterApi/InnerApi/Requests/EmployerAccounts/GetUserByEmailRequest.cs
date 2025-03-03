using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;

public class GetUserByEmailRequest : IGetApiRequest
{
    public string Email { get; }

    public GetUserByEmailRequest(string email)
    {
        Email = email;
    }

    public string GetUrl => $"api/user?email={Email}";
}