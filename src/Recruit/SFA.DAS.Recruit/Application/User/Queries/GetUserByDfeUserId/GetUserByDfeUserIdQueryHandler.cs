using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.User.Queries.GetUserByDfeUserId;

public class GetUserByDfeUserIdQueryHandler(IRecruitApiClient<RecruitApiConfiguration> apiClient) : IRequestHandler<GetUserByDfeUserIdQuery, GetUserByDfeUserIdQueryResult>
{
    public async Task<GetUserByDfeUserIdQueryResult> Handle(GetUserByDfeUserIdQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.GetWithResponseCode<GetUserByDfeUserIdResponse>(new GetUserByDfeUserIdRequest(request.DfeUserId));
        return response.StatusCode == HttpStatusCode.NotFound
            ? new GetUserByDfeUserIdQueryResult(null)
            : new GetUserByDfeUserIdQueryResult(response.Body);
    }
}