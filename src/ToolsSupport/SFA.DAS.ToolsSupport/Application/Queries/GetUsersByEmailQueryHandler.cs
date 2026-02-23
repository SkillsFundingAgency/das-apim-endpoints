using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Application.Queries;
public class GetUsersByEmailQueryHandler(IInternalApiClient<EmployerProfilesApiConfiguration> client)
    : IRequestHandler<GetUsersByEmailQuery, GetUsersByEmailQueryResult>
{
    public async Task<GetUsersByEmailQueryResult> Handle(GetUsersByEmailQuery request, CancellationToken cancellationToken)
    {
        var response = await client.Get<GetUsersByEmailResponse>(new GetUsersByEmailRequest(request.Email));
        return new GetUsersByEmailQueryResult { Users = response.Users };
    }
}

