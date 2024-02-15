using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests.DeleteJobRequest;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteJob
{
    public class PostDeleteTrainingCourseCommandHandler : IRequestHandler<PostDeleteTrainingCourseCommand, Unit>
    {
        private readonly ICandidateApiClient<CandidateApiConfiguration> _apiClient;

        public PostDeleteTrainingCourseCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        {
            _apiClient = candidateApiClient;
        }

        public async Task<Unit> Handle(PostDeleteTrainingCourseCommand command, CancellationToken cancellationToken)
        {
            var request = new DeleteTrainingCourseRequest(command.ApplicationId, command.CandidateId, command.TrainingCourseId);

            await _apiClient.Delete(request);
            return Unit.Value;
        }
    }

}
