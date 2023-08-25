using System.Net;
using MediatR;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMember;

public class GetEmployerMemberQueryHandler : IRequestHandler<GetEmployerMemberQuery, GetEmployerMemberQueryResult?>
{
    private readonly IAanHubRestApiClient _aanHubApiClient;

    public GetEmployerMemberQueryHandler(IAanHubRestApiClient aanHubApiClient)
    {
        _aanHubApiClient = aanHubApiClient;
    }

    public async Task<GetEmployerMemberQueryResult?> Handle(GetEmployerMemberQuery request, CancellationToken cancellationToken)
    {
        var response = await _aanHubApiClient.GetEmployer(request.UserRef, cancellationToken);
        var result = response.ResponseMessage.StatusCode switch
        {
            HttpStatusCode.OK => response.GetContent(),
            HttpStatusCode.NotFound => null,
            _ => throw new InvalidOperationException($"Get employer member didn't come back with successful response")
        };
        return result;
    }
}