using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;
public class PutCandidateCommandHandler : IRequestHandler<PutCandidateCommand, PutCandidateCommandResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public PutCandidateCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _candidateApiClient = candidateApiClient;
    }
    public async Task<PutCandidateCommandResult> Handle(PutCandidateCommand request, CancellationToken cancellationToken)
    {
        var putData = new PutCandidateApiRequest.PutCandidateApiRequestData
        {
            Email = request.Email
        };
        var putRequest = new PutCandidateApiRequest(request.GovUkIdentifier, putData);

        var candidateResult = await _candidateApiClient.PutWithResponseCode<PutCandidateApiResponse>(putRequest);

        candidateResult.EnsureSuccessStatusCode();

        if (candidateResult is null) return null;

        return new PutCandidateCommandResult()
        {
            Id = candidateResult.Body.Id,
            GovUkIdentifier = candidateResult.Body.GovUkIdentifier,
            Email = candidateResult.Body.Email,
            FirstName = candidateResult.Body.FirstName,
            LastName = candidateResult.Body.LastName
        };
    }
}
