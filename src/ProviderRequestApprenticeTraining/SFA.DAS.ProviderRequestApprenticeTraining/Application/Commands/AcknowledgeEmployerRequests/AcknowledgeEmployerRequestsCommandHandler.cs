using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.AcknowledgeEmployerRequests
{
    public class AcknowledgeEmployerRequestsCommandHandler : IRequestHandler<AcknowledgeEmployerRequestsCommand,Unit>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;

        public AcknowledgeEmployerRequestsCommandHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
        }

        public async Task<Unit> Handle(AcknowledgeEmployerRequestsCommand command, CancellationToken cancellationToken)
        {
            var request = new AcknowledgeEmployerRequestsRequest(new AcknowledgeEmployerRequestsData
            {
                EmployerRequestIds = command.EmployerRequestIds,
                Ukprn = command.Ukprn
            });

            var response = await _requestApprenticeTrainingApiClient
                .PostWithResponseCode<AcknowledgeEmployerRequestsData, AcknowledgeEmployerRequestsResponse>(request, false);

            response.EnsureSuccessStatusCode();
            return Unit.Value;
        }
    }
}
