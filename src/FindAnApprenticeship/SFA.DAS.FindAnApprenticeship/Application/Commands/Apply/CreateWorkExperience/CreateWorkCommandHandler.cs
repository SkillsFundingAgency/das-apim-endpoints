using MediatR;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateWorkExperience;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateJob;

public class CreateWorkCommandHandler(ICandidateApiClient<CandidateApiConfiguration> apiClient)
    : IRequestHandler<CreateWorkCommand, CreateWorkCommandResponse>
{
    public async Task<CreateWorkCommandResponse> Handle(CreateWorkCommand command, CancellationToken cancellationToken)
    {
        var requestBody = new PostWorkHistoryApiRequest.PostWorkHistoryApiRequestData
        {
            EmployerName = command.EmployerName,
            JobDescription = command.JobDescription,
            JobTitle = command.JobTitle,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            WorkHistoryType = WorkHistoryType.WorkExperience
        };
        var request = new PostWorkHistoryApiRequest(command.ApplicationId, command.CandidateId, requestBody);

        var response = await apiClient.PostWithResponseCode<PostWorkHistoryApiResponse>(request);

        response.EnsureSuccessStatusCode();

        return new CreateWorkCommandResponse
        {
            Id = response.Body.Id
        };
    }
}