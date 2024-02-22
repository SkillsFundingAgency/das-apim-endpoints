using MediatR;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateWorkExperience;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateJob;

public class CreateWorkCommandHandler(ICandidateApiClient<CandidateApiConfiguration> apiClient)
    : IRequestHandler<CreateWorkCommand, CreateWorkCommandResponse>
{
    public async Task<CreateWorkCommandResponse> Handle(CreateWorkCommand command, CancellationToken cancellationToken)
    {
        var requestBody = new PutUpsertWorkHistoryApiRequest.PutUpsertWorkHistoryApiRequestData
        {
            Employer = command.CompanyName,
            Description = command.Description,
            JobTitle = string.Empty,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            WorkHistoryType = WorkHistoryType.WorkExperience
        };

        var request = new PutUpsertWorkHistoryApiRequest(command.ApplicationId, command.CandidateId, Guid.NewGuid(), requestBody);

        var response = await apiClient.PutWithResponseCode<PutUpsertWorkHistoryApiResponse>(request);

        response.EnsureSuccessStatusCode();

        return new CreateWorkCommandResponse
        {
            Id = response.Body.Id
        };
    }
}