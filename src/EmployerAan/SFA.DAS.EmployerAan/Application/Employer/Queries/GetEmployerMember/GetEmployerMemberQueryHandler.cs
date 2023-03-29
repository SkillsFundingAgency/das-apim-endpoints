using MediatR;
using SFA.DAS.EmployerAan.Configuration;
using SFA.DAS.EmployerAan.InnerApi.Requests;
using SFA.DAS.EmployerAan.Services;

namespace SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMember;

public class GetEmployerMemberQueryHandler : IRequestHandler<GetEmployerMemberQuery, GetEmployerMemberQueryResult>
{
    private readonly IAanHubApiClient<AanHubApiConfiguration> _aanHubApiClient;

    public GetEmployerMemberQueryHandler(IAanHubApiClient<AanHubApiConfiguration> aanHubApiClient)
    {
        _aanHubApiClient = aanHubApiClient;
    }

    public Task<GetEmployerMemberQueryResult> Handle(GetEmployerMemberQuery request, CancellationToken cancellationToken)
    {
        return _aanHubApiClient.Get<GetEmployerMemberQueryResult>(new GetEmployerMemberRequest() { UserRef = request.UserRef });
    }
}