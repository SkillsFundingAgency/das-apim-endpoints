using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

using EmployerRequest = SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining.EmployerRequest;

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
                GetWithResponseCode<EmployerRequest>(new GetEmployerRequestRequest(request.EmployerRequestId));

            employerRequest.EnsureSuccessStatusCode();

            return new GetEmployerRequestResult
            {
                EmployerRequest = (SharedOuterApi.Models.RequestApprenticeTraining.EmployerRequest)employerRequest.Body
            };
        }
    }
}
