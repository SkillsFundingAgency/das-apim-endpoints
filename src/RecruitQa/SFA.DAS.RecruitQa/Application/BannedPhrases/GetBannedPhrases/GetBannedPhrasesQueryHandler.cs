using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.Application.BannedPhrases.GetBannedPhrases;

public class GetBannedPhrasesQueryHandler(IRecruitApiClient recruitApiClient) : IRequestHandler<GetBannedPhrasesQuery, GetBannedPhrasesQueryResult>
{
    public async Task<GetBannedPhrasesQueryResult> Handle(GetBannedPhrasesQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.Get<List<string>>(new GetProhibitedcontentByContentTypeApiRequest(ProhibitedContentType.BannedPhrases));
        return new GetBannedPhrasesQueryResult(response);
    }
}