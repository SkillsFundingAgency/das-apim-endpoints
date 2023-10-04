using MediatR;
using SFA.DAS.EmployerAan.Application.Services;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Application.Members.Queries.GetMembers;

public class GetMembersQueryHandler : IRequestHandler<GetMembersQuery, GetMembersQueryResult?>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetMembersQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetMembersQueryResult?> Handle(GetMembersQuery request,
        CancellationToken cancellationToken)
    {
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(request);
        return await _apiClient.GetMembers(parameters, cancellationToken);
    }
}
