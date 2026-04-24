using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateTrainingCourse;
public class CreateTrainingCourseCommandHandler : IRequestHandler<CreateTrainingCourseCommand, CreateTrainingCourseCommandResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _apiClient;

    public CreateTrainingCourseCommandHandler(ICandidateApiClient<CandidateApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<CreateTrainingCourseCommandResult?> Handle(CreateTrainingCourseCommand command, CancellationToken cancellationToken)
    {
        var requestBody = new PutUpsertTrainingCourseApiRequest.PutUpdateTrainingCourseApiRequestData
        {
            CourseName = command.CourseName,
            YearAchieved = command.YearAchieved
        };
        var request = new PutUpsertTrainingCourseApiRequest(command.ApplicationId, command.CandidateId, Guid.NewGuid(), requestBody);

        var result = await _apiClient.PutWithResponseCode<PutUpsertTrainingCourseApiResponse>(request);
        result.EnsureSuccessStatusCode();

        if (result is null) return null;

        return new CreateTrainingCourseCommandResult
        {
            Id = result.Body.Id
        };
    }
}
