using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.ExpireEmployerRequests
{
    public class ExpireEmployerRequestsCommandHandler : IRequestHandler<ExpireEmployerRequestsCommand, ExpireEmployerRequestsResponse>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;

        public ExpireEmployerRequestsCommandHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
        }

        public async Task<ExpireEmployerRequestsResponse> Handle(ExpireEmployerRequestsCommand command, CancellationToken cancellationToken)
        {
            var response = await _requestApprenticeTrainingApiClient
               .PostWithResponseCode<ExpireEmployerRequestsData, ExpireEmployerRequestsResponse>(new ExpireEmployerRequestsRequest(), false);

            response.EnsureSuccessStatusCode();
            return response.Body;
        }
    }
}
