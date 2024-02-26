using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetCandidateAdditionalQuestionTwo;
public class GetCandidateAdditionalQuestionTwoQueryHandler : IRequestHandler<GetCandidateAdditionalQuestionTwoQuery, GetCandidateAdditionalQuestionTwoQueryResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public GetCandidateAdditionalQuestionTwoQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _candidateApiClient = candidateApiClient;
    }

    public async Task<GetCandidateAdditionalQuestionTwoQueryResult> Handle(GetCandidateAdditionalQuestionTwoQuery request, CancellationToken cancellationToken)
    {
        return await _candidateApiClient.Get<GetCandidateAdditionalQuestionTwoApiResponse>
            (new GetCandidateAdditionalQuestionTwoApiRequest(request.ApplicationId, request.CandidateId));
    }
}
