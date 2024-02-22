using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteVolunteering;
public class PostDeleteVolunteeringOrWorkExperienceCommandHandler(
    ICandidateApiClient<CandidateApiConfiguration> apiClient)
    : IRequestHandler<PostDeleteVolunteeringOrWorkExperienceCommand, Unit>
{
    public async Task<Unit> Handle(PostDeleteVolunteeringOrWorkExperienceCommand command, CancellationToken cancellationToken)
    {
        var request = new PostDeleteJobApiRequest(command.ApplicationId, command.CandidateId, command.Id);

        await apiClient.Delete(request);
        return Unit.Value;
    }
}
