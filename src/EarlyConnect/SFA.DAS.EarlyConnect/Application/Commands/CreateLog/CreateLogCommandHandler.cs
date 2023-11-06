using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateLog
{
    public class CreateLogCommandHandler : IRequestHandler<CreateLogCommand, Unit>
    {

        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public CreateLogCommandHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<Unit> Handle(CreateLogCommand request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.PostWithResponseCode<InnerApi.Requests.CreateLog>(new CreateLogRequest(request.Log), false);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
