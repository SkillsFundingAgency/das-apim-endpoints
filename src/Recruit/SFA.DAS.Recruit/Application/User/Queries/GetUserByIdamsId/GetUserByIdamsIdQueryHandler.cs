using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.User.Queries.GetUserByIdamsId;

public class GetUserByIdamsIdQueryHandler(IRecruitApiClient<RecruitApiConfiguration> apiClient) : IRequestHandler<GetUserByIdamsIdQuery, GetUserByIdamsIdQueryResult>
{
    public async Task<GetUserByIdamsIdQueryResult> Handle(GetUserByIdamsIdQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.GetWithResponseCode<GetUserByIdamsIdResponse>(new GetUserByIdamsIdRequest(request.IdamsId));
        return response.StatusCode == HttpStatusCode.NotFound
            ? new GetUserByIdamsIdQueryResult(null)
            : new GetUserByIdamsIdQueryResult(response.Body);
    }
}