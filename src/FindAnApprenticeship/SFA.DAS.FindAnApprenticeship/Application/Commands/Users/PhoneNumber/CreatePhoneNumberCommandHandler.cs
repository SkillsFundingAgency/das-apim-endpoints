using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.PhoneNumber;
public class CreatePhoneNumberCommandHandler : IRequestHandler<CreatePhoneNumberCommand, Unit>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public CreatePhoneNumberCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _candidateApiClient = candidateApiClient;
    }
    public async Task<Unit> Handle(CreatePhoneNumberCommand request, CancellationToken cancellationToken)
    {
        var postRequest = new PutCandidateApiRequest(request.CandidateId, new PutCandidateApiRequestData
        {
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        });

        var response = await _candidateApiClient.PutWithResponseCode<PutCandidateApiResponse>(postRequest);

        if ((int)response.StatusCode > 300)
        {
            throw new InvalidOperationException();
        }

        return Unit.Value;
    }
}
