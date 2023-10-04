using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.Members.Queries.GetMember;

public class GetMemberQueryHandler : IRequestHandler<GetMemberQuery, GetMemberQueryResult?>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetMemberQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetMemberQueryResult?> Handle(GetMemberQuery request, CancellationToken cancellationToken)
    {
        return await _apiClient.GetMember(request.MemberId, cancellationToken);
    }
}

