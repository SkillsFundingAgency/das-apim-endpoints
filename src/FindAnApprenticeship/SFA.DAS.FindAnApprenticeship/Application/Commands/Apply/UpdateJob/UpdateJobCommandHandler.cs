using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateJob;

public class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand, UpdateJobCommandResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _apiClient;

    public UpdateJobCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _apiClient = candidateApiClient;
    }

    public async Task<UpdateJobCommandResult?> Handle(UpdateJobCommand command, CancellationToken cancellationToken)
    {
        var requestBody = new PutUpsertWorkHistoryApiRequest.PutUpsertWorkHistoryApiRequestData
        {
            Employer = command.Employer,
            Description = command.Description,
            JobTitle = command.JobTitle,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            WorkHistoryType = WorkHistoryType.Job
        };
        var request = new PutUpsertWorkHistoryApiRequest(command.ApplicationId, command.CandidateId, command.JobId, requestBody);

        var result = await _apiClient.PutWithResponseCode<PutUpsertWorkHistoryApiResponse>(request);
        result.EnsureSuccessStatusCode();

        if (result is null) return null;

        return new UpdateJobCommandResult
        {
            Id = result.Body.Id
        };
    }
}