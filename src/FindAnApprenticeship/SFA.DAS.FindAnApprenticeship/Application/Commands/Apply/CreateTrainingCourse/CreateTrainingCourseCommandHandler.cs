using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateTrainingCourse;
public class CreateTrainingCourseCommandHandler : IRequestHandler<CreateTrainingCourseCommand, CreateTrainingCourseCommandResponse>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _apiClient;

    public CreateTrainingCourseCommandHandler(ICandidateApiClient<CandidateApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<CreateTrainingCourseCommandResponse> Handle(CreateTrainingCourseCommand command, CancellationToken cancellationToken)
    {
        var requestBody = new PostTrainingCourseApiRequest.PostTrainingCourseApiRequestData
        {
            CourseName = command.CourseName,
            TrainingProviderName = command.TrainingProviderName,
            YearAchieved = command.YearAchieved
        };
        var request = new PostTrainingCourseApiRequest(command.ApplicationId, command.CandidateId, requestBody);

        var response = await _apiClient.PostWithResponseCode<PostTrainingCourseApiResponse>(request);
        response.EnsureSuccessStatusCode();

        return new CreateTrainingCourseCommandResponse
        {
            Id = response.Body.Id
        };
    }
}
