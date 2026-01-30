using MediatR;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.Application.Profanity.GetProfanity;

public class GetProfanityListQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) : IRequestHandler<GetProfanityListQuery, GetProfanityListQueryResult>
{
    public async Task<GetProfanityListQueryResult> Handle(GetProfanityListQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.Get<List<string>>(new GetProfanityListApiRequest());
        return GetProfanityListQueryResult.FromInnerApiResponse(response);
    }
}