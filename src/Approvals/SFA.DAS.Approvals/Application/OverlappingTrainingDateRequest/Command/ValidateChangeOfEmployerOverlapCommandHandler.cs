using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;

namespace SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Command
{
    public class ValidateChangeOfEmployerOverlapCommandHandler : IRequestHandler<ValidateChangeOfEmployerOverlapCommand>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public ValidateChangeOfEmployerOverlapCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<Unit> Handle(ValidateChangeOfEmployerOverlapCommand request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.PostWithResponseCode<object>(
               new PostValidateChangeOfEmployerOverlapRequest(request.ValidateChangeOfEmployerOverlapRequest), false);

            result.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
