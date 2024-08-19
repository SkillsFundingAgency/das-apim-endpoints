using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequest
{
    public class CreateProviderResponseEmployerRequestCommandHandler : IRequestHandler<CreateProviderResponseEmployerRequestCommand>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;

        public CreateProviderResponseEmployerRequestCommandHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
        }

        public async Task Handle(CreateProviderResponseEmployerRequestCommand command, CancellationToken cancellationToken)
        {
            var request = new CreateProviderResponseEmployerRequestRequest(new CreateEmployerResponseEmployerRequestData
            {
                EmployerRequestIds = command.EmployerRequestIds,
                Ukprn = command.Ukprn
            });

            var response = await _requestApprenticeTrainingApiClient
                .PostWithResponseCode<CreateEmployerResponseEmployerRequestData, CreateProviderResponseEmployerRequestResponse>(request, false);

            response.EnsureSuccessStatusCode();
        }
    }
}
