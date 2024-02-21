using RestEase;
using SFA.DAS.EmployerAan.Application.User.Commands;
using SFA.DAS.EmployerAan.Application.User.GetUserAccounts;


namespace SFA.DAS.EmployerAan.Infrastructure;
public interface IEmployerProfilesApiClient : IHealthChecker
{
    [Get("api/users/{id}")]
    [AllowAnyStatusCode]
    Task<Response<EmployerProfileUsersApiResponse>> GetEmployerUserAccount([Path] string id, CancellationToken cancellationToken);



    [Put("api/users/{id}")]
    [AllowAnyStatusCode]
    Task<Response<EmployerProfileUsersApiResponse>> PutEmployerUserAccount([Path] string id, [Body] PutEmployerProfileRequest request, CancellationToken cancellationToken);
}
