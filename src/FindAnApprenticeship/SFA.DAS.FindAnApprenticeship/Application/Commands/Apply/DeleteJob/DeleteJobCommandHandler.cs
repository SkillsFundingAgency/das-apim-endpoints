using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteJob
{
    public class DeleteJobCommandHandler : IRequestHandler<DeleteJobCommand>
    {
        private readonly ICandidateApiClient<CandidateApiConfiguration> _apiClient;

        public DeleteJobCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        {
            _apiClient = candidateApiClient;
        }

        public async Task<Unit> Handle(DeleteJobCommand command, CancellationToken cancellationToken)
        {
            var request = new DeleteJobRequest(command.ApplicationId, command.CandidateId, command.JobId);

            await _apiClient.Delete(request);
            return Unit.Value;
        }
    }

}
