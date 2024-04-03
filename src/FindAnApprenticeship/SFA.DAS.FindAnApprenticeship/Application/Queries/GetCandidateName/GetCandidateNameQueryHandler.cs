using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateName;
public class GetCandidateNameQueryHandler : IRequestHandler<GetCandidateNameQuery, GetCandidateNameQueryResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public GetCandidateNameQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _candidateApiClient = candidateApiClient;
    }

    public async Task<GetCandidateNameQueryResult> Handle(GetCandidateNameQuery request, CancellationToken cancellationToken)
    {
        return await _candidateApiClient.Get<GetCandidateNameApiResponse>(new GetCandidateNameApiRequest(request.GovUkIdentifier));
    }
}
