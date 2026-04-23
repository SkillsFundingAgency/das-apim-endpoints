using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Recruit.Application.User.Queries.GetUserByEmail;

public class GetUserByEmailQueryHandler(IRecruitApiClient<RecruitApiConfiguration> apiClient) : IRequestHandler<GetUserByEmailQuery, GetUserByEmailQueryResult>
{
    public async Task<GetUserByEmailQueryResult> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.PostWithResponseCode<GetUserByEmailResponse>(new GetUserByEmailRequest(request.Email, request.UserType));
        return response.StatusCode == HttpStatusCode.NotFound
            ? new GetUserByEmailQueryResult(null)
            : new GetUserByEmailQueryResult(response.Body);
    }
}