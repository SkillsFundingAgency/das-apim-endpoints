using MediatR;
using SFA.DAS.EmployerAan.Configuration;
using SFA.DAS.EmployerAan.InnerApi.Profiles;
using SFA.DAS.EmployerAan.Services;

namespace SFA.DAS.EmployerAan.Application.Profiles.Queries.GetProfilesByUserType;

public class GetProfilesByUserTypeQueryHandler : IRequestHandler<GetProfilesByUserTypeQuery, GetProfilesByUserTypeQueryResult?>
{
    private readonly IAanHubApiClient<AanHubApiConfiguration> _apiClient;
    public GetProfilesByUserTypeQueryHandler(IAanHubApiClient<AanHubApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }
    public async Task<GetProfilesByUserTypeQueryResult?> Handle(GetProfilesByUserTypeQuery request, CancellationToken cancellationToken)
    {
        return await _apiClient.Get<GetProfilesByUserTypeQueryResult>(new GetProfilesByUserTypeRequest(request.UserType));
    }
}
