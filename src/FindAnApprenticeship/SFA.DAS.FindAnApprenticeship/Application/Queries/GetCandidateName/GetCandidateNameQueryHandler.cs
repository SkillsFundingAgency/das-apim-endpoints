using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateName;
public class GetCandidateNameQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    : IRequestHandler<GetCandidateNameQuery, GetCandidateNameQueryResult>
{
    public async Task<GetCandidateNameQueryResult> Handle(GetCandidateNameQuery request, CancellationToken cancellationToken)
    {
        return await candidateApiClient.Get<GetCandidateApiResponse>(new GetCandidateApiRequest(request.CandidateId));
    }
}
