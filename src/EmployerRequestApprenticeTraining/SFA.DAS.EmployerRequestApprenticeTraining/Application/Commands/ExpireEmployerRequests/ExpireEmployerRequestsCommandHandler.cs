using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.ExpireEmployerRequests
{
    public class ExpireEmployerRequestsCommandHandler : IRequestHandler<ExpireEmployerRequestsCommand, Unit>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;

        public ExpireEmployerRequestsCommandHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
        }

        public async Task<Unit> Handle(ExpireEmployerRequestsCommand command, CancellationToken cancellationToken)
        {
            var response = await _requestApprenticeTrainingApiClient
               .PostWithResponseCode<ExpireEmployerRequestsData, object>(new ExpireEmployerRequestsRequest(), false);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
