using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddress;
public class GetCandidatePostcodeQueryHandler : IRequestHandler<GetCandidatePostcodeQuery, GetCandidatePostcodeQueryResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public GetCandidatePostcodeQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _candidateApiClient = candidateApiClient;
    }

    public async Task<GetCandidatePostcodeQueryResult> Handle(GetCandidatePostcodeQuery request, CancellationToken cancellationToken)
    {
        return await _candidateApiClient.Get<GetCandidatePostcodeApiResponse>(new GetCandidatePostcodeApiRequest(request.CandidateId));
    }
}
