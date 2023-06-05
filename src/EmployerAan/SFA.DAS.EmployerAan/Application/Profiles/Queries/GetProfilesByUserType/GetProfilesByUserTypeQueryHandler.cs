using MediatR;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Application.Profiles.Queries.GetProfilesByUserType;

public class GetProfilesByUserTypeQueryHandler : IRequestHandler<GetProfilesByUserTypeQuery, GetProfilesByUserTypeQueryResult?>
{
    private readonly IAanHubRestApiClient _apiClient;
    public GetProfilesByUserTypeQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetProfilesByUserTypeQueryResult?> Handle(GetProfilesByUserTypeQuery request, CancellationToken cancellationToken)
    {
        return await _apiClient.GetProfiles(request.UserType, cancellationToken);
    }
}
