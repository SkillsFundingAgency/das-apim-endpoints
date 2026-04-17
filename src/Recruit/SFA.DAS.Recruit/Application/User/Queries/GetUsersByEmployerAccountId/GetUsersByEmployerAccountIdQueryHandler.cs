using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Recruit.Application.User.Queries.GetUsersByEmployerAccountId;

public class GetUsersByEmployerAccountIdQueryHandler(IRecruitApiClient<RecruitApiConfiguration> apiClient) : IRequestHandler<GetUsersByEmployerAccountIdQuery, GetUsersByEmployerAccountIdQueryResult>
{
    public async Task<GetUsersByEmployerAccountIdQueryResult> Handle(GetUsersByEmployerAccountIdQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.GetWithResponseCode<GetUsersByEmployerAccountIdResponse>(new GetUsersByEmployerAccountIdRequest(request.EmployerAccountId));
        return response.StatusCode == HttpStatusCode.NotFound
            ? new GetUsersByEmployerAccountIdQueryResult(null)
            : new GetUsersByEmployerAccountIdQueryResult(response.Body);
    }
}