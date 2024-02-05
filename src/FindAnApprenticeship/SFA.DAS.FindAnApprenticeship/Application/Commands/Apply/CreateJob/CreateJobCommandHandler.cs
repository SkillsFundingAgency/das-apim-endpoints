using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateJob;

public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, CreateJobCommandResponse>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _apiClient;

    public CreateJobCommandHandler(ICandidateApiClient<CandidateApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<CreateJobCommandResponse> Handle(CreateJobCommand command, CancellationToken cancellationToken)
    {
        var requestBody = new PostWorkHistoryApiRequest.PostWorkHistoryApiRequestData
        {
            EmployerName = command.EmployerName,
            JobDescription = command.JobDescription,
            JobTitle = command.JobTitle,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            WorkHistoryType = WorkHistoryType.Job
        };
        var request = new PostWorkHistoryApiRequest(command.ApplicationId, command.CandidateId, requestBody);

        var response = await _apiClient.PostWithResponseCode<PostWorkHistoryApiResponse>(request);
        response.EnsureSuccessStatusCode();

        return new CreateJobCommandResponse
        {
            Id = response.Body.Id
        };
    }
}