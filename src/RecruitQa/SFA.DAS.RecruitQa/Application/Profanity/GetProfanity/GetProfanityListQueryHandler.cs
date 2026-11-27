using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.Application.Profanity.GetProfanity;

public class GetProfanityListQueryHandler(IRecruitApiClient recruitApiClient) : IRequestHandler<GetProfanityListQuery, GetProfanityListQueryResult>
{
    public async Task<GetProfanityListQueryResult> Handle(GetProfanityListQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.Get<List<string>>(new GetProhibitedcontentByContentTypeApiRequest(ProhibitedContentType.Profanity));
        return new GetProfanityListQueryResult(response);
    }
}