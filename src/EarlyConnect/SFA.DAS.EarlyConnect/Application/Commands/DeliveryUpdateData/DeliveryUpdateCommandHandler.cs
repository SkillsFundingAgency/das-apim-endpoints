using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.DeliveryUpdateData
{
    public class DeliveryUpdateCommandHandler : IRequestHandler<DeliveryUpdateCommand, DeliveryUpdateCommandResult>
    {

        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public DeliveryUpdateCommandHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<DeliveryUpdateCommandResult> Handle(DeliveryUpdateCommand request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.PostWithResponseCode<DeliveryUpdateDataResponse>(new DeliveryUpdateRequest(request.DeliveryUpdate));

            response.EnsureSuccessStatusCode();

            return new DeliveryUpdateCommandResult
            {
                Message = response.Body.Message,
            };
        }
    }
}
