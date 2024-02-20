using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteVolunteeringOrWorkExperience;
public class PostDeleteVolunteeringOrWorkExperienceCommandHandler : IRequestHandler<PostDeleteVolunteeringOrWorkExperienceCommand, Unit>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _apiClient;

    public PostDeleteVolunteeringOrWorkExperienceCommandHandler(ICandidateApiClient<CandidateApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<Unit> Handle(PostDeleteVolunteeringOrWorkExperienceCommand command, CancellationToken cancellationToken)
    {
        var request = new PostDeleteJobApiRequest(command.ApplicationId, command.CandidateId, command.Id);

        await _apiClient.Delete(request);
        return Unit.Value;
    }
}
