using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetDateOfBirth;
public class GetDateOfBirthQueryHandler : IRequestHandler<GetDateOfBirthQuery, GetDateOfBirthQueryResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public GetDateOfBirthQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _candidateApiClient = candidateApiClient;
    }

    public async Task<GetDateOfBirthQueryResult> Handle(GetDateOfBirthQuery request, CancellationToken cancellationToken)
    {
        return await _candidateApiClient.Get<GetCandidateDateOfBirthApiResponse>(new GetCandidateDateOfBirthApiRequest(request.GovUkIdentifier));
    }
}
