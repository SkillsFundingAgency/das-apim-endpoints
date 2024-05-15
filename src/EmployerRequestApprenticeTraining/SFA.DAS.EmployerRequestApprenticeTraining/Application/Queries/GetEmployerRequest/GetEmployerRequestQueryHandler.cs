using MediatR;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

using EmployerRequest = SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Responses.EmployerRequest;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequest
{
    public class GetEmployerRequestQueryHandler : IRequestHandler<GetEmployerRequestQuery, GetEmployerRequestResult>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;

        public GetEmployerRequestQueryHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
        }

        public async Task<GetEmployerRequestResult> Handle(GetEmployerRequestQuery request, CancellationToken cancellationToken)
        {
            var employerRequest = await _requestApprenticeTrainingApiClient.
                Get<EmployerRequest>(new GetEmployerRequestRequest(request.EmployerRequestId));

            return new GetEmployerRequestResult
            {
                EmployerRequest = (Models.EmployerRequest)employerRequest
            };
        }
    }
}
