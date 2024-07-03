using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateEqualityQuestionsCommand;
public class UpsertAboutYouEqualityQuestionsCommandHandler(
    ICandidateApiClient<CandidateApiConfiguration> apiClient)
    : IRequestHandler<UpsertAboutYouEqualityQuestionsCommand, UpsertAboutYouEqualityQuestionsCommandResult>
{
    public async Task<UpsertAboutYouEqualityQuestionsCommandResult> Handle(UpsertAboutYouEqualityQuestionsCommand command, CancellationToken cancellationToken)
    {
        var requestBody = new PutUpsertAboutYouItemApiRequest.PutUpdateAboutYouItemApiRequestData
        {
            Sex = command.Sex,
            EthnicGroup = command.EthnicGroup,
            EthnicSubGroup = command.EthnicSubGroup,
            IsGenderIdentifySameSexAtBirth = command.IsGenderIdentifySameSexAtBirth,
            OtherEthnicSubGroupAnswer = command.OtherEthnicSubGroupAnswer,
        };
        var request = new PutUpsertAboutYouItemApiRequest(command.CandidateId, requestBody);

        var putResult = await apiClient.PutWithResponseCode<PutUpsertAboutYouItemApiResponse>(request);
        putResult.EnsureSuccessStatusCode();

        if (putResult is null) return null;

        return new UpsertAboutYouEqualityQuestionsCommandResult
        {
            Id = putResult.Body.Id,
        };
    }
}
