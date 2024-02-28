using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateAdditionalQuestion;
public class UpdateAdditionalQuestionCommandHandler : IRequestHandler<UpdateAdditionalQuestionCommand, UpdateAdditionalQuestionQueryResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _apiClient;

    public UpdateAdditionalQuestionCommandHandler(ICandidateApiClient<CandidateApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<UpdateAdditionalQuestionQueryResult> Handle(UpdateAdditionalQuestionCommand command, CancellationToken cancellationToken)
    {
        var requestBody = new PutUpsertAdditionalQuestionApiRequest.PutUpsertAdditionalQuestionApiRequestData
        {
            Answer = command.Answer,
            QuestionId = command.QuestionId
        };
        var request = new PutUpsertAdditionalQuestionApiRequest(command.ApplicationId, command.CandidateId, requestBody);

        var result = await _apiClient.PutWithResponseCode<PutUpsertAdditionalQuestionApiResponse>(request);
        result.EnsureSuccessStatusCode();

        if (result is null) return null;

        return new UpdateAdditionalQuestionQueryResult
        {
            Id = result.Body.Id,
        };
    }
}
