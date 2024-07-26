using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.UpdateProviderResponseStatus
{
    public class UpdateProviderResponseStatusCommandHandler : IRequestHandler<UpdateProviderResponseStatusCommand, UpdateProviderResponseStatusResponse>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;

        public UpdateProviderResponseStatusCommandHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
        }

        public async Task<UpdateProviderResponseStatusResponse> Handle(UpdateProviderResponseStatusCommand command, CancellationToken cancellationToken)
        {
            var request = new UpdateProviderResponseStatusRequest(new UpdateProviderResponseStatusData
            {
                EmployerRequestIds = command.EmployerRequestIds,
                Ukprn = command.Ukprn,
                ProviderResponseStatus = command.ProviderResponseStatus,
            });

            var response = await _requestApprenticeTrainingApiClient
                .PostWithResponseCode<UpdateProviderResponseStatusData, UpdateProviderResponseStatusResponse>(request);

            response.EnsureSuccessStatusCode();
            return response.Body;
        }
    }
}
