using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Candidate.GetCandidateDetails;

public record GetCandidateDetailsQueryHandler : IRequestHandler<GetCandidateDetailsQuery, GetCandidateDetailsQueryResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public GetCandidateDetailsQueryHandler(
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _candidateApiClient = candidateApiClient;
    }

    public async Task<GetCandidateDetailsQueryResult> Handle(
        GetCandidateDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var candidateTask =
            _candidateApiClient.Get<GetCandidateApiResponse>(
                new GetCandidateApiRequest(request.CandidateId.ToString()));
        var addressTask =
            _candidateApiClient.Get<GetAddressApiResponse>(
                new GetCandidateAddressApiRequest(request.CandidateId));

        await Task.WhenAll(candidateTask, addressTask);

        var candidate = candidateTask.Result;
        var address = addressTask.Result;

        if (candidate is null) return null;

        return new GetCandidateDetailsQueryResult
        {
            Id = candidate.Id,
            FirstName = candidate.FirstName,
            LastName = candidate.LastName,
            Email = candidate.Email,
            GovUkIdentifier = candidate.GovUkIdentifier,
            MiddleName = candidate.MiddleName,
            PhoneNumber = candidate.PhoneNumber,
            Address = address
        };
    }
}