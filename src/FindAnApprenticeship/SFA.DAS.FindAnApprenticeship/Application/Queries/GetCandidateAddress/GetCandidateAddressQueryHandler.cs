using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddress;
public class GetCandidateAddressQueryHandler : IRequestHandler<GetCandidateAddressQuery, GetCandidateAddressQueryResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public GetCandidateAddressQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _candidateApiClient = candidateApiClient;
    }

    public async Task<GetCandidateAddressQueryResult> Handle(GetCandidateAddressQuery request, CancellationToken cancellationToken)
    {
        return await _candidateApiClient.Get<GetCandidateAddressApiResponse>(new GetCandidateAddressApiRequest(request.CandidateId));
    }
}
