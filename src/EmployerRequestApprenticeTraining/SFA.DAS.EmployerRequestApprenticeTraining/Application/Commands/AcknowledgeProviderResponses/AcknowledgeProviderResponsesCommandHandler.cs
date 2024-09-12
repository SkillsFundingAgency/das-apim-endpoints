using MediatR;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses
{
    public class AcknowledgeProviderResponsesCommandHandler : IRequestHandler<AcknowledgeProviderResponsesCommand>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;

        public AcknowledgeProviderResponsesCommandHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
        }

        public async Task Handle(AcknowledgeProviderResponsesCommand command, CancellationToken cancellationToken)
        {
            var request = new PutAcknowledgeProviderResponsesRequest(command.EmployerRequestId, new PutAcknowledgeProviderResponsesRequestData
            {
                AcknowledgedBy = command.AcknowledgedBy
            });

            await _requestApprenticeTrainingApiClient
                .Put(request);
        }
    }
}
