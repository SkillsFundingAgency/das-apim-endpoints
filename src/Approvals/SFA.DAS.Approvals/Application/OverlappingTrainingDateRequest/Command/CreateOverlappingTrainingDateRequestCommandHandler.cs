using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Command
{
    public class CreateOverlappingTrainingDateRequestCommandHandler : IRequestHandler<CreateOverlappingTrainingDateRequestCommand, CreateOverlappingTrainingDateResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IMediator _mediator;

        public CreateOverlappingTrainingDateRequestCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IMediator mediator)
        {
            _apiClient = apiClient;
            _mediator = mediator;
        }

        public async Task<CreateOverlappingTrainingDateResult> Handle(CreateOverlappingTrainingDateRequestCommand request, CancellationToken cancellationToken)
        {
           var result = await _apiClient.PostWithResponseCode<CreateOverlappingTrainingDateResult>(
                 new PostCreateOverlappingTrainingDateRequest(request.ProviderId, request.DraftApprenticeshipId, request.UserInfo));

            result.EnsureSuccessStatusCode();

            return result.Body;
        }
    }
}
