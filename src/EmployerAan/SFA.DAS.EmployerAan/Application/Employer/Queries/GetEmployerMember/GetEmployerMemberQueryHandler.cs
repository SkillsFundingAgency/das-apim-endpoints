using System.Net;
using MediatR;
using SFA.DAS.EmployerAan.Configuration;
using SFA.DAS.EmployerAan.InnerApi.EmployerMembers;
using SFA.DAS.EmployerAan.Services;

namespace SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMember;

public class GetEmployerMemberQueryHandler : IRequestHandler<GetEmployerMemberQuery, GetEmployerMemberQueryResult?>
{
    private readonly IAanHubApiClient<AanHubApiConfiguration> _aanHubApiClient;

    public GetEmployerMemberQueryHandler(IAanHubApiClient<AanHubApiConfiguration> aanHubApiClient)
    {
        _aanHubApiClient = aanHubApiClient;
    }

    public async Task<GetEmployerMemberQueryResult?> Handle(GetEmployerMemberQuery request, CancellationToken cancellationToken)
    {
        var response = await _aanHubApiClient.GetWithResponseCode<GetEmployerMemberQueryResult>(new GetEmployerMemberRequest() { UserRef = request.UserRef });
        var result = response.StatusCode switch
        {
            HttpStatusCode.OK => response.Body,
            HttpStatusCode.NotFound => null,
            _ => throw new InvalidOperationException($"Get employer member didn't some back with successful response")
        };
        return result;
    }
}