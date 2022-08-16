using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Command
{
    public class ValidateDraftApprenticeshipDetailsCommandHandler : IRequestHandler<ValidateDraftApprenticeshipDetailsCommand>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public ValidateDraftApprenticeshipDetailsCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<Unit> Handle(ValidateDraftApprenticeshipDetailsCommand request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.PostWithResponseCode<object>(
               new PostValidateDraftApprenticeshipDetailsRequest(request.DraftApprenticeshipRequest), false);

            result.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
