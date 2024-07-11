using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
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
            ApiResponse<EmployerRequest> employerRequest = null;
            if(request.EmployerRequestId.HasValue)
            {
                employerRequest = await _requestApprenticeTrainingApiClient.
                    GetWithResponseCode<EmployerRequest>(new GetEmployerRequestRequest(request.EmployerRequestId.Value));
            }
            else if(request.AccountId.HasValue && !string.IsNullOrEmpty(request.StandardReference))
            {
                employerRequest = await _requestApprenticeTrainingApiClient.
                    GetWithResponseCode<EmployerRequest>(new GetEmployerRequestRequest(request.AccountId.Value, request.StandardReference));
            }

            if (employerRequest?.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                employerRequest.EnsureSuccessStatusCode();
            }

            return new GetEmployerRequestResult
            {
                EmployerRequest = (SharedOuterApi.Models.RequestApprenticeTraining.EmployerRequest)employerRequest?.Body
            };
        }
    }
}
