using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateJob;

public class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _apiClient;

    public UpdateJobCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _apiClient = candidateApiClient;
    }

    public async Task<Unit> Handle(UpdateJobCommand command, CancellationToken cancellationToken)
    {
        var requestBody = new PutUpdateWorkHistoryApiRequest.PutUpdateWorkHistoryApiRequestData
        {
            Employer = command.Employer,
            Description = command.Description,
            JobTitle = command.JobTitle,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            WorkHistoryType = WorkHistoryType.Job
        };
        var request = new PutUpdateWorkHistoryApiRequest(command.ApplicationId, command.CandidateId, command.JobId, requestBody);

        await _apiClient.Put(request);
        return Unit.Value;
    }
}