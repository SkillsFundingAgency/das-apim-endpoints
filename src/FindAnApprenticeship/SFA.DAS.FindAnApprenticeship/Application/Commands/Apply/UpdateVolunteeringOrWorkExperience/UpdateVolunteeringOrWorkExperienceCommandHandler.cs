using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateVolunteeringOrWorkExperience;

public class UpdateVolunteeringOrWorkExperienceCommandHandler(
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    : IRequestHandler<UpdateVolunteeringOrWorkExperienceCommand, UpdateVolunteeringOrWorkExperienceCommandResult>
{
    public async Task<UpdateVolunteeringOrWorkExperienceCommandResult?> Handle(UpdateVolunteeringOrWorkExperienceCommand command, CancellationToken cancellationToken)
    {
        var requestBody = new PutUpsertWorkHistoryApiRequest.PutUpsertWorkHistoryApiRequestData
        {
            Employer = command.Employer,
            Description = command.Description,
            JobTitle = string.Empty,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            WorkHistoryType = WorkHistoryType.WorkExperience
        };
        var request = new PutUpsertWorkHistoryApiRequest(command.ApplicationId, command.CandidateId, command.Id, requestBody);

        var result = await candidateApiClient.PutWithResponseCode<PutUpsertWorkHistoryApiResponse>(request);

        result.EnsureSuccessStatusCode();

        if (result is null) return null;

        return new UpdateVolunteeringOrWorkExperienceCommandResult
        {
            Id = result.Body.Id
        };
    }
}