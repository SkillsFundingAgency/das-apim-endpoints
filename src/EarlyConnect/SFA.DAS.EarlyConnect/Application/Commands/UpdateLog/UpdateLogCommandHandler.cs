using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.UpdateLog
{
    public class UpdateLogCommandHandler : IRequestHandler<UpdateLogCommand, Unit>
    {

        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public UpdateLogCommandHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<Unit> Handle(UpdateLogCommand request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.PostWithResponseCode<InnerApi.Requests.UpdateLog>(new UpdateLogRequest(request.Log), false);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
