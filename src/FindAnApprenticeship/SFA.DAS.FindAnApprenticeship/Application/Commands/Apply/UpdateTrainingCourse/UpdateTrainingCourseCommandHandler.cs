using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateTrainingCourse;
public class UpdateTrainingCourseCommandHandler : IRequestHandler<UpdateTrainingCourseCommand, UpdateTrainingCourseCommandResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _apiClient;

    public UpdateTrainingCourseCommandHandler(ICandidateApiClient<CandidateApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<UpdateTrainingCourseCommandResult?> Handle(UpdateTrainingCourseCommand command, CancellationToken cancellationToken)
    {
        var requestBody = new PutUpsertTrainingCourseApiRequest.PutUpdateTrainingCourseApiRequestData
        {
            CourseName = command.CourseName,
            YearAchieved = command.YearAchieved
        };
        var request = new PutUpsertTrainingCourseApiRequest(command.ApplicationId, command.CandidateId, command.TrainingCourseId, requestBody);

        var result = await _apiClient.PutWithResponseCode<PutUpsertTrainingCourseApiResponse>(request);
        result.EnsureSuccessStatusCode();

        if (result is null) return null;

        return new UpdateTrainingCourseCommandResult
        {
            Id = result.Body.Id
        };
    }
}
