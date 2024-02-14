using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateJob;

public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _apiClient;

    public CreateJobCommandHandler(ICandidateApiClient<CandidateApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<Unit> Handle(CreateJobCommand command, CancellationToken cancellationToken)
    {
        var requestBody = new PutUpsertWorkHistoryApiRequest.PutUpsertWorkHistoryApiRequestData
        {
            Employer = command.EmployerName,
            Description = command.JobDescription,
            JobTitle = command.JobTitle,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            WorkHistoryType = WorkHistoryType.Job
        };
        var request = new PutUpsertWorkHistoryApiRequest(command.ApplicationId, command.CandidateId, Guid.NewGuid(), requestBody);

        await _apiClient.Put(request);

        return Unit.Value;
    }
}
