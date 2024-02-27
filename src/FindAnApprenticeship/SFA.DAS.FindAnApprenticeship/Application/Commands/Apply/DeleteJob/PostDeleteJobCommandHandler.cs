using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteJob
{
    public class PostDeleteJobCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        : IRequestHandler<PostDeleteJobCommand, Unit>
    {
        public async Task<Unit> Handle(PostDeleteJobCommand command, CancellationToken cancellationToken)
        {
            var request = new PostDeleteJobApiRequest(command.ApplicationId, command.CandidateId, command.JobId);

            await candidateApiClient.Delete(request);
            return Unit.Value;
        }
    }
}
