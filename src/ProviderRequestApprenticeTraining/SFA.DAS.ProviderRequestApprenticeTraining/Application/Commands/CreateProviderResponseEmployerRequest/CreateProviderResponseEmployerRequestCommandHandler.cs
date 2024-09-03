using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequest
{
    public class CreateProviderResponseEmployerRequestCommandHandler : IRequestHandler<CreateProviderResponseEmployerRequestCommand,Unit>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;

        public CreateProviderResponseEmployerRequestCommandHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
        }

        public async Task<Unit> Handle(CreateProviderResponseEmployerRequestCommand command, CancellationToken cancellationToken)
        {
            var request = new CreateProviderResponseEmployerRequestRequest(new CreateEmployerResponseEmployerRequestData
            {
                EmployerRequestIds = command.EmployerRequestIds,
                Ukprn = command.Ukprn
            });

            var response = await _requestApprenticeTrainingApiClient
                .PostWithResponseCode<CreateEmployerResponseEmployerRequestData, object>(request, false);

            response.EnsureSuccessStatusCode();
            return Unit.Value;
        }
    }
}
