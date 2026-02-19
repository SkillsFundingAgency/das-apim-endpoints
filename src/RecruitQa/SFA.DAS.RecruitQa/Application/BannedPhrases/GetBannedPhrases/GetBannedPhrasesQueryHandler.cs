using MediatR;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.Application.BannedPhrases.GetBannedPhrases;

public class GetBannedPhrasesQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) : IRequestHandler<GetBannedPhrasesQuery, GetBannedPhrasesQueryResult>
{
    public async Task<GetBannedPhrasesQueryResult> Handle(GetBannedPhrasesQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.Get<List<string>>(new GetBannedPhrasesListApiRequest());
        return GetBannedPhrasesQueryResult.FromInnerApiResponse(response);
    }
}