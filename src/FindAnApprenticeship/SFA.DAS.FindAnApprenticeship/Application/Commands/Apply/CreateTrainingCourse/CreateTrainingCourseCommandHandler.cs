using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateTrainingCourse;
public class CreateTrainingCourseCommandHandler : IRequestHandler<CreateTrainingCourseCommand>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _apiClient;

    public CreateTrainingCourseCommandHandler(ICandidateApiClient<CandidateApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<Unit> Handle(CreateTrainingCourseCommand command, CancellationToken cancellationToken)
    {
        var requestBody = new PutUpsertTrainingCourseApiRequest.PutUpdateTrainingCourseApiRequestData
        {
            CourseName = command.CourseName,
            YearAchieved = command.YearAchieved
        };
        var request = new PutUpsertTrainingCourseApiRequest(command.ApplicationId, command.CandidateId, Guid.NewGuid(), requestBody);

        await _apiClient.Put(request);
        return Unit.Value;
    }
}
