using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.CheckAnswers;

public class UpdateCheckAnswersCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<UpdateCheckAnswersCommand>
{
    public async Task Handle(UpdateCheckAnswersCommand request, CancellationToken cancellationToken)
    {
        var postRequest = new PutCandidateApiRequest(request.CandidateId, new PutCandidateApiRequestData
        {
            Status = UserStatus.Completed
        });

        var response = await candidateApiClient.PutWithResponseCode<PutCandidateApiResponse>(postRequest);

        if ((int)response.StatusCode > 300)
        {
            throw new InvalidOperationException();
        }
    }
}