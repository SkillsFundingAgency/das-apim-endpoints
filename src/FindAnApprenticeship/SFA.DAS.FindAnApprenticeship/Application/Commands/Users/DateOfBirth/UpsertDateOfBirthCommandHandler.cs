using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.DateOfBirth;
public class UpsertDateOfBirthCommandHandler : IRequestHandler<UpsertDateOfBirthCommand, Unit>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public UpsertDateOfBirthCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _candidateApiClient = candidateApiClient;
    }

    public async Task<Unit> Handle(UpsertDateOfBirthCommand command, CancellationToken cancellationToken)
    {
        var putData = new PutCandidateApiRequestData
        {
            DateOfBirth = command.DateOfBirth,
            Email = command.Email
        };

        var putRequest = new PutCandidateApiRequest(command.CandidateId, putData);

        var response = await _candidateApiClient.PutWithResponseCode<NullResponse>(putRequest);

        if ((int)response.StatusCode > 300)
        {
            throw new InvalidOperationException();
        }

        return Unit.Value;
    }
}
